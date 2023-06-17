using XRest.Orders.Contracts.Responses.Basket;
using XRest.Shared.Extensions;

namespace XRest.Orders.App.Services;

public class MenuProductDetailsCreator : IMenuProductDetailsCreator
{
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;

	public MenuProductDetailsCreator(IReadOnlyProductRepository readOnlyProductRepository)
	{
		_readOnlyProductRepository = readOnlyProductRepository;
	}

	public ProductDetails? CreateFromProduct(int productId, string? lang)
	{
		Domain.ReadOnlyProduct? product = _readOnlyProductRepository.GetReadOnlyProductById(productId);

		if (product == null)
		{
			return null;
		}

		return CreateFromProduct(product, lang);
	}

	public ProductDetails? CreateFromProduct(Domain.ReadOnlyProduct product, string? lang)
	{
		ProductDetails? details = null;
		switch (product.Type)
		{
			case Domain.ProductType.Product:
				details = UnpackSingle(product, lang);
				break;
		}

		return details;
	}

	private static ProductDetails UnpackBundle(Domain.ReadOnlyProduct product, string? lang)
	{
		List<ProductSet> productSets = new();
		List<ProductBundle> bundles = new();

		var newProduct = new ProductDetails
		{
			ProductId = product.Id,
			Type = (ProductType)product.Type,
			Title = product.GetTranslatedDisplayName(lang),
			Bundles = MapBundle(product.Bundles, lang),
			NotFound = false,
			Price = product.Price,
			ImageUrl = $"{product.Id}&ts={product.ImageAudit?.Ticks ?? 0}",
		};

		var bundlesGroups = product.Bundles!.GroupBy(x => x.Label).ToList();
		foreach (var bundleGroup in bundlesGroups)
		{
			if (bundleGroup.Count() == 1)
			{
				var bundleItem = bundleGroup.First();
				if (bundleItem.Product!.ProductSets != null && bundleItem.Product.ProductSets.Length > 0)
				{
					productSets.AddRange(MapProductSets(bundleItem.Product, bundleGroup.Key, lang));
				}
				else
				{
					bundles.Add(new ProductBundle
					{
						Label = bundleItem.LabelTranslations?.GetTranslate(lang, bundleItem.Label ?? string.Empty),
						Items = new ProductBundleItem[]
						 {
							 new ProductBundleItem
							 {
								  Price = bundleItem.Price ?? bundleItem.Product.Price,
								  ProductId = bundleItem.ProductId,
								  Selected = bundleItem.Default,
								  Title = bundleItem.Product.GetTranslatedDisplayName(lang)
							 }
						 }
					});
				}
			}
			else
			{
				bundles.Add(new ProductBundle
				{
					Label = bundleGroup.Key,
					Items = bundleGroup
							.Select(x => new ProductBundleItem
							{
								Price = x.Price ?? x.Product!.Price,
								ProductId = x.ProductId,
								Selected = x.Default,
								Title = x.Product!.GetTranslatedDisplayName(lang)
							})
							.ToArray()
				});
			}
		}

		newProduct.Bundles = bundles.ToArray();
		newProduct.ProductSets = productSets.ToArray();

		return newProduct;
	}
	private static ProductBundle[] MapBundle(Domain.ReadOnlyProductBundle[]? bundles, string? lang)
	{
		if (bundles == null || bundles.Length == 0)
		{
			return Array.Empty<ProductBundle>();
		}

		return bundles
			.GroupBy(static x => x.Label)
			.Select(x => new ProductBundle
			{
				Label = x.Key,
				Items = x
					.Select(y => new ProductBundleItem
					{
						ProductId = y.ProductId,
						Title = y.Product!.GetTranslatedDisplayName(lang),
						Selected = y.Default,
						Price = y.Price ?? y.Product.Price
					})
					.ToArray()
			})
			.ToArray();
	}

	private static ProductDetails UnpackSingle(Domain.ReadOnlyProduct product, string? lang)
	{
		var newProduct = new ProductDetails
		{
			ProductId = product.Id,
			Type = (ProductType)product.Type,
			Title = product.GetTranslatedDisplayName(lang),
			Bundles = Array.Empty<ProductBundle>(),
			NotFound = false,
			Price = product.Price,
			ImageUrl = $"{product.Id}&ts={product.ImageAudit?.Ticks ?? 0}",
			ProductSets = MapProductSets(product, null, lang)
		};

		return newProduct;
	}

	private static ProductSet[] MapProductSets(Domain.ReadOnlyProduct product, string? bundleLabel, string? lang)
	{
		if (product.ProductSets == null || product.ProductSets.Length == 0)
		{
			return Array.Empty<ProductSet>();
		}

		return product.ProductSets
			.Select(x => new ProductSet
			{
				Id = x.Id,
				Title = x.DisplayName,
				Required = x.Type == Domain.ProductSetType.Required,
				MainProductId = product.Id,
				BundleLabel = bundleLabel,
				Items = x.Items!
							.OrderBy(y => y.Order)
							.Select(y => new ProductSetItem
							{
								Id = y.Id,
								Selected = false,
								Title = y.Product!.GetTranslatedDisplayName(lang),
								ProductId = y.ProductId
							})
							.ToArray()
			})
			.ToArray();
	}
}
