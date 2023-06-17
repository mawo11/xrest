using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Basket.GetItemDetails;

public sealed class GetItemDetailsQueryHandler : IRequestHandler<GetItemDetailsQuery, ProductDetails>
{
	private static readonly ProductDetails NotFound = new() { NotFound = true };

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<GetItemDetailsQueryHandler> _logger;
	private readonly IMenuProductDetailsCreator _menuProductDetailsCreator;

	public GetItemDetailsQueryHandler(
		IBasketStorage basketStorage,
		ILogger<GetItemDetailsQueryHandler> logger,
		IMenuProductDetailsCreator menuProductDetailsCreator)
	{
		_menuProductDetailsCreator = menuProductDetailsCreator;
		_basketStorage = basketStorage;
		_logger = logger;
	}

	public Task<ProductDetails> Handle(GetItemDetailsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetItemDetailsQueryHandler -> basketKey: {BasketKey} ItemId: {ItemId}",
			request.BasketKey,
			request.ItemId);

		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Task.FromResult(NotFound);
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null)
		{
			return Task.FromResult(NotFound);
		}

		var basketItem = basketData.Items.Find(x => x.Id == request.ItemId);
		if (basketItem == null)
		{
			return Task.FromResult(NotFound);
		}

		var productDetails = _menuProductDetailsCreator.CreateFromProduct(basketItem.Product);

		ConfigureSingleProduct(productDetails!, basketItem.SelectedProducts);
		ConfigureBundleProduct(productDetails!, basketItem.SelectedProducts);

		productDetails!.Id = basketItem.Id;

		return Task.FromResult(productDetails);
	}

	private static void ConfigureBundleProduct(ProductDetails productDetails, BasketItemSelectedProduct selectedProducts)
	{
		if (productDetails.Bundles == null)
		{
			return;
		}

		foreach (var bundle in productDetails.Bundles)
		{
			foreach (var bundleItem in bundle.Items!)
			{
				var selected = selectedProducts.SubProducts!.Any(x => x.Label == bundle.Label && x.Id == bundleItem.ProductId);
				bundleItem.Selected = selected;
			}
		}
	}

	private static void ConfigureSingleProduct(ProductDetails productDetails, BasketItemSelectedProduct selectedProducts)
	{
		foreach (var productSet in productDetails.ProductSets!)
		{
			foreach (var productSetItem in productSet.Items!)
			{
				bool selected = selectedProducts.SubProducts!.Any(
							x => x.Id == productSet.MainProductId &&
							x.ProductSets != null &&
							x.ProductSets.Any(y => y.ProductSetId == productSet.Id && y.ProductSetItemId == productSetItem.Id));

				productSetItem.Selected = selected;
			}
		}

	}
}
