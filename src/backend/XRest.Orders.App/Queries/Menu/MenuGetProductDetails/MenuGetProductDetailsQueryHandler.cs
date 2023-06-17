
using XRest.Orders.App.Queries.Menu.GetMenuGroupProductsForRestauantAndGroup;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Menu.MenuGetProductDetails;
public sealed class MenuGetProductDetailsQueryHandler : IRequestHandler<MenuGetProductDetailsQuery, ProductDetails>
{
	private static readonly ProductDetails NotFound = new() { NotFound = true };

	private readonly IMenuProductDetailsCreator _menuProductDetailsCreator;
	private readonly ILogger<GetMenuGroupProductsForRestauantAndGroupQueryHandler> _logger;

	public MenuGetProductDetailsQueryHandler(IMenuProductDetailsCreator menuProductDetailsCreator,
		ILogger<GetMenuGroupProductsForRestauantAndGroupQueryHandler> logger)
	{
		_menuProductDetailsCreator = menuProductDetailsCreator;
		_logger = logger;
	}

	public Task<ProductDetails> Handle(MenuGetProductDetailsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug($"BasketGetProductDetailsQueryHandler: prodId({request.ProductId}) ");

		var details = _menuProductDetailsCreator.CreateFromProduct(request.ProductId, request.Lang);

		return Task.FromResult(details ?? NotFound);
	}
}
