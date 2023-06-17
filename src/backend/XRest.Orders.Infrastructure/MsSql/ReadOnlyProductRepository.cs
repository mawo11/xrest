using Dapper;
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Extensions.Caching.Memory;

namespace XRest.Orders.Infrastructure.MsSql;

internal sealed class ReadOnlyProductRepository : IReadOnlyProductRepository
{
	private readonly ILogger<ReadOnlyProductRepository> _logger;
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly List<ReadOnlyProduct> _products;
	private readonly List<ReadOnlyProductGroup> _groups;
	private readonly ReaderWriterLockSlim _cacheLock;
	private readonly Dictionary<int, ReadOnlyProductGroup[]> _restaurantGroups;
	private readonly List<ReadOnlyProductSet> _productSets;
	private readonly List<int> _activeRestaurantIds;
	private readonly List<ReadonlyVat> _vats;

	public ReadOnlyProductRepository(ILogger<ReadOnlyProductRepository> logger, ISqlConnectionFactory sqlConnectionFactory)
	{
		_logger = logger;
		_sqlConnectionFactory = sqlConnectionFactory;
		_products = new List<ReadOnlyProduct>();
		_groups = new List<ReadOnlyProductGroup>();
		_restaurantGroups = new Dictionary<int, ReadOnlyProductGroup[]>();
		_productSets = new List<ReadOnlyProductSet>();
		_cacheLock = new ReaderWriterLockSlim();
		_activeRestaurantIds = new List<int>();
		_vats = new List<ReadonlyVat>();
	}

	public void LoadAllProducts()
	{
		try
		{
			using var connection = _sqlConnectionFactory.MakeConnection();
			_activeRestaurantIds.Clear();
			_activeRestaurantIds.AddRange(connection.Query<int>("ord.GetActiveRestaurant", commandType: CommandType.StoredProcedure));
			_logger.LogInformation("Start aktualizacji produktow");
			LoadGroups(connection);
			LoadProducts(connection);
			LoadVats(connection);
			_logger.LogInformation("koniec aktualizacji produktow");
		}
		catch (Exception ex)
		{
			_logger.LogCritical("{message}", ex.Message);
		}
	}

	public ReadOnlyProductGroup[] GetGroupsForRestaurant(int restaurantId)
	{
		_cacheLock.EnterReadLock();
		try
		{
			if (_restaurantGroups.ContainsKey(restaurantId))
			{
				return _restaurantGroups[restaurantId];
			}

			return Array.Empty<ReadOnlyProductGroup>();
		}
		finally
		{
			_cacheLock.ExitReadLock();
		}
	}

	public IReadOnlyList<ReadOnlyProduct> GetAllProducts()
	{
		_cacheLock.EnterReadLock();
		try
		{
			return _products;
		}
		finally
		{
			_cacheLock.ExitReadLock();
		}
	}

	public IReadOnlyList<ReadonlyVat> LoadAllVats() => _vats;

	public ReadOnlyProduct? GetReadOnlyProductById(int productId)
	{
		_cacheLock.EnterReadLock();
		try
		{
			return _products.Find(x => x.Id == productId);
		}
		finally
		{
			_cacheLock.ExitReadLock();
		}
	}

	private void LoadVats(SqlConnection connection)
	{
		var result = connection.Query<ReadonlyVat>("ord.GetAllVat", commandType: CommandType.StoredProcedure);
		_cacheLock.EnterWriteLock();
		try
		{
			_vats.Clear();
			_vats.AddRange(result);
		}
		catch (Exception e)
		{
			_logger.LogError("{message}", e.Message);
		}
		finally
		{
			_cacheLock.ExitWriteLock();
		}
	}

	private void LoadGroups(SqlConnection connection)
	{
		try
		{
			var resultSet = connection.QueryMultiple("ord.GetAllGroups", commandType: CommandType.StoredProcedure);

			IEnumerable<ReadOnlyProductGroup> productGroups = resultSet.Read<ReadOnlyProductGroup>();
			IEnumerable<ProductGroupsRestaurant> productGroupsRestaurants = resultSet.Read<ProductGroupsRestaurant>();

			if (!productGroups.Any() || productGroupsRestaurants == null || !productGroupsRestaurants.Any())
			{
				return;
			}

			_cacheLock.EnterWriteLock();
			try
			{
				_groups.Clear();
				_groups.AddRange(productGroups);
			}
			catch (Exception e)
			{
				_logger.LogError("{message}", e.Message);
			}
			finally
			{
				_cacheLock.ExitWriteLock();
			}

			_restaurantGroups.Clear();

			foreach (var activeRestaurantId in _activeRestaurantIds)
			{
				_restaurantGroups.Add(activeRestaurantId, productGroupsRestaurants
											.Where(x => x.RestaurantId == activeRestaurantId)
											.OrderBy(static x => x.Index)
											.Select(x => _groups.FirstOrDefault(y => y.Id == x.ProductGroupId))
											.ToArray()!);
			}
		}
		catch (Exception e)
		{
			_logger.LogCritical("{message}", e.Message);
		}
	}

	private void LoadProducts(SqlConnection connection)
	{
		try
		{
			var resultSet = connection.QueryMultiple("ord.GetAllProductSets", commandType: CommandType.StoredProcedure);
			IEnumerable<ProductSet> productSets = resultSet.Read<ProductSet>();
			IEnumerable<ProductSetItem> productSetItems = resultSet.Read<ProductSetItem>();

			resultSet = connection.QueryMultiple("ord.GetAllProducts", commandType: CommandType.StoredProcedure);

			IEnumerable<ReadOnlyProduct> readOnlyProducts = resultSet.Read<ReadOnlyProduct>();
			IEnumerable<ReadOnlyProductBundle> productBundles = resultSet.Read<ReadOnlyProductBundle>();
			IEnumerable<ProductSetProduct> productSetProduct = resultSet.Read<ProductSetProduct>();
			IEnumerable<ReadOnlyRestaurantProduct> restaurantProducts = resultSet.Read<ReadOnlyRestaurantProduct>();

			_cacheLock.EnterWriteLock();

			_products.Clear();
			_products.AddRange(readOnlyProducts);

			_productSets.Clear();
			_productSets.AddRange(productSets
									.Select(x => new ReadOnlyProductSet
									{
										Id = x.Id,
										DisplayName = x.DisplayName,
										DisplayNameTranslations = x.DisplayNameTranslations,
										Name = x.Name,
										Type = x.Type,
										Items = productSetItems
													.Where(y => y.ProductSetsId == x.Id)
													.OrderBy(static y => y.Order)
													.Select(y => new ReadOnlyProductSetItem
													{
														Id = y.Id,
														Order = y.Order,
														Amount = y.Amount,
														ProductId = y.ProductId,
														Product = _products.Find(a => a.Id == y.ProductId)!
													})
													.ToArray()

									}).ToList());


			foreach (var productBundle in productBundles)
			{
				productBundle.Product = _products.Find(x => x.Id == productBundle.ProductId)!;
			}

			foreach (var product in readOnlyProducts)
			{
				product.ProductGroup = _groups.Find(x => x.Id == product.ProductGroupId)!;
				if (product.ProductGroup.Type == ProductGroupType.System)
				{
					continue;
				}

				if (product.PackageId.HasValue)
				{
					product.Package = _products.Find(x => x.Id == product.PackageId.Value);
				}

				if (string.IsNullOrEmpty(product.DisplayNameTranslations))
				{
					product.DisplayNameTranslations = $"<langs><pl>{product.DisplayName}</pl><en>{product.DisplayName}</en></langs>";
				}

				switch (product.Type)
				{
					case ProductType.Product:
						product.ProductSets = productSetProduct
											.Where(x => x.ProductId == product.Id)
											.OrderBy(static x => x.Order)
											.Select(x => MapProductSetForProduct(x))
											.ToArray();
						product.Bundles = Array.Empty<ReadOnlyProductBundle>();
						break;
				}

				List<ReadOnlyRestaurantContext> items = new();
				foreach (var activeRestaurantId in _activeRestaurantIds)
				{
					ReadOnlyRestaurantContext readOnlyRestaurantContext = new ReadOnlyRestaurantContext
					{
						Data = restaurantProducts.FirstOrDefault(x => x.RestaurantId == activeRestaurantId && x.ProductId == product.Id),
						RestaurantId = activeRestaurantId
					};

					items.Add(readOnlyRestaurantContext);
				}

				product.RestaurantSpecificData.Clear();
				product.RestaurantSpecificData.AddRange(items);
			}
		}
		catch (Exception e)
		{
			_logger.LogError("{message}", e.Message);
		}
		finally
		{
			_cacheLock.ExitWriteLock();
		}
	}

	private ReadOnlyProductSet MapProductSetForProduct(ProductSetProduct x)
	{
		var result = _productSets.Find(y => y.Id == x.ProductSetId)!;

		return new ReadOnlyProductSet
		{
			RawId = x.Id,
			DisplayName = result.DisplayName,
			DisplayNameTranslations = result.DisplayName,
			Id = result.Id,
			Items = result.Items,
			Name = result.Name,
			Type = result.Type
		};
	}
}
