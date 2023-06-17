using XRest.Orders.App.Domain;
using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using XRest.Restaurants.Contracts;

namespace XRest.Orders.App.Services;

public class BasketViewGenerator : IBasketViewGenerator
{
	private readonly BasketView EmptyView = new()
	{
		Items = Array.Empty<BasketViewItem>()
	};

	private readonly IRestaurantService _restaurantService;
	private readonly IOrderTotalCalculateService _orderTotalCalculateService;

	public BasketViewGenerator(IRestaurantService restaurantService, IOrderTotalCalculateService orderTotalCalculateService)
	{
		_orderTotalCalculateService = orderTotalCalculateService;
		_restaurantService = restaurantService;
	}

	public async ValueTask<BasketView> GenerateAsync(BasketData basketData, string? lang)
	{
		await _orderTotalCalculateService.CalculateAsync(basketData);
		var groups = basketData.Items.GroupBy(static x => x.Id);

		List<BasketViewItem> items = new();
		List<BasketViewItemBundleItem> bundles = new();

		foreach (var group in groups)
		{
			var firstItem = group.First();

			if (firstItem.SelectedProducts == null)
			{
				continue;
			}

			var basketItem = new BasketViewItem
			{
				Count = group.Count(),
				Id = group.Key,
				Note = firstItem.SelectedProducts != null ? firstItem.SelectedProducts.Note : null,
				Title = firstItem.Product.GetTranslatedDisplayName(lang),
			};

			decimal price = firstItem.Total;
			bundles.Clear();

			if (firstItem.SelectedProducts!.SubProducts != null)
			{
				switch (firstItem.Product.Type)
				{
					case Domain.ProductType.Product:
						basketItem.SubProducts = GenerateSubProducts(
							firstItem.Product.ProductSets!,
							firstItem.SelectedProducts.SubProducts.FirstOrDefault(x => x.Id == firstItem.Product.Id),
							lang);

						break;

				}
			}

			if (firstItem.Product.Package != null)
			{
				bundles.Add(new BasketViewItemBundleItem
				{
					Title = firstItem.Product.Package.GetTranslatedDisplayName(lang),
				});
			}

			basketItem.Bundles = bundles.ToArray();
			decimal totalPrice = price * group.Count();
			basketItem.Price = totalPrice.ToString("C", CultureSettings.DefaultCultureInfo);

			items.Add(basketItem);
		}

		if (basketData.BagProduct != null && items.Count > 0)
		{
			items.Add(new BasketViewItem
			{
				ReadOnly = true,
				Count = 1,
				Id = null,
				Price = basketData.BagProduct.Price.ToString("C", CultureSettings.DefaultCultureInfo),
				Title = basketData.BagProduct.GetTranslatedDisplayName(lang)
			});
		}

		if (basketData.DiscountCode != null)
		{
			items.Add(new BasketViewItem
			{
				ReadOnly = true,
				Count = 1,
				Id = null,
				Price = basketData.DiscountCode.Value + " %",
				Title = "Rabat: " + basketData.DiscountCode.Code
			});
		}

		if (basketData.GratisProduct != null)
		{
			items.Add(new BasketViewItem
			{
				ReadOnly = true,
				Count = 1,
				Id = null,
				Price = basketData.GratisProduct.Total.ToString("C", CultureSettings.DefaultCultureInfo),
				Title = basketData.GratisProduct.Product.GetTranslatedDisplayName(lang)
			});

			if (basketData.GratisProduct.Product.Package != null)
			{
				items.Add(new BasketViewItem
				{
					Title = basketData.GratisProduct.Product.Package.GetTranslatedDisplayName(lang),
					ReadOnly = true,
					Count = 1,
					Id = null,
					Price = basketData.GratisProduct.Product.Package.Price.ToString("C", CultureSettings.DefaultCultureInfo),
				});
			}
		}

		var restaurantData = await _restaurantService.GetInformationForOrderingAsync(basketData.RestaurantId);

		if (restaurantData == null)
		{
			return EmptyView;
		}

		return new BasketView
		{
			IsDelivey = basketData.Type == Domain.DeliveryType.Delivery,
			Items = items.ToArray(),
			Total = basketData.Total.ToString("C", CultureSettings.DefaultCultureInfo),
			Transport = basketData.DeliveryPrice > 0 ? basketData.DeliveryPrice.ToString("C", CultureSettings.DefaultCultureInfo) : null,
			CanSubmit = basketData.Type == Domain.DeliveryType.Personal ||
					(basketData.Type == Domain.DeliveryType.Delivery && basketData.TotalWithoutDelivery >= restaurantData.MinOrder)
		};
	}

	private static BasketViewItemBundleItem[] MapBundles(ReadOnlyProductBundle[]? bundles, BasketSubProduct[] subProducts, string? lang)
	{
		if (bundles == null || bundles.Length == 0)
		{
			return Array.Empty<BasketViewItemBundleItem>();
		}

		List<BasketViewItemBundleItem> items = new();
		foreach (var bundle in bundles
			.OrderBy(x => x.Label)
			.Where(x => !string.IsNullOrEmpty(x.Label))
			.GroupBy(x => x.Label))
		{
			var bundleProduct = subProducts.FirstOrDefault(x => x.Label == bundle.Key);
			if (bundleProduct == null)
			{
				continue;
			}

			var product = bundle.FirstOrDefault(x => x.ProductId == bundleProduct.Id);
			if (product != null && product.Product != null)
			{
				items.Add(new BasketViewItemBundleItem
				{
					Title = product.Product.GetTranslatedDisplayName(lang),
					SubProducts = GenerateSubProducts(product.Product.ProductSets, bundleProduct, lang)
				});
			}
		}

		foreach (var bundle in bundles.Where(x => string.IsNullOrEmpty(x.Label)))
		{
			var bundleProduct = subProducts.FirstOrDefault(x => x.Id == bundle.ProductId);
			if (bundleProduct == null)
			{
				continue;
			}

			var product = bundle.Product;
			items.Add(new BasketViewItemBundleItem
			{
				Title = product!.GetTranslatedDisplayName(lang),
				SubProducts = GenerateSubProducts(product.ProductSets, bundleProduct, lang)
			});
		}

		return items.ToArray();
	}

	private static string[] GenerateSubProducts(ReadOnlyProductSet[]? productSets, BasketSubProduct? basketSubProduct, string? lang)
	{
		if (basketSubProduct == null || productSets == null || basketSubProduct.ProductSets == null)
		{
			return Array.Empty<string>();
		}

		List<string> items = new();
		foreach (var productSet in productSets)
		{
			if (productSet.Items != null)
			{
				foreach (var productSetItem in productSet.Items)
				{
					if (basketSubProduct.ProductSets.Any(x => x.ProductSetId == productSet.Id && x.ProductSetItemId == productSetItem.Id))
					{
						string? temp = productSetItem.Product!.GetTranslatedDisplayName(lang);
						if (!string.IsNullOrEmpty(temp))
						{
							items.Add(temp);
						}
					}
				}
			}
		}

		if (items.Count > 0)
		{
			return items.ToArray();
		}

		return Array.Empty<string>();
	}
}
