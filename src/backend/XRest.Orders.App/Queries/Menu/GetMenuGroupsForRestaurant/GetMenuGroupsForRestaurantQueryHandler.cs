using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Menu;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Menu.GetMenuGroupsForRestaurant;

public sealed class GetMenuGroupsForRestaurantQueryHandler : IRequestHandler<GetMenuGroupsForRestaurantQuery, MenuGroup[]>
{
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;
	private readonly ILogger<GetMenuGroupsForRestaurantQueryHandler> _logger;

	public GetMenuGroupsForRestaurantQueryHandler(IReadOnlyProductRepository readOnlyProductRepository,
		ILogger<GetMenuGroupsForRestaurantQueryHandler> logger)
	{
		_logger = logger;
		_readOnlyProductRepository = readOnlyProductRepository;
	}

	public Task<MenuGroup[]> Handle(GetMenuGroupsForRestaurantQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("ReGetMenuGroupsForRestaurantQueryHandler: " + request.RestaurantId);

		var products = _readOnlyProductRepository.GetAllProducts();

		var groups = _readOnlyProductRepository.GetGroupsForRestaurant(request.RestaurantId)
						.Where(x => products.Any(y => y.ProductGroupId == x.Id && y.IsAllowedOnlineForRestaurant(request.RestaurantId)))
						.Select(x => new MenuGroup
						{
							Id = x.Id,
							Name = x.NameTranslations!.GetTranslate(request.Lang, x.Name)
						})
						.ToArray();


		return Task.FromResult(groups);
	}
}
