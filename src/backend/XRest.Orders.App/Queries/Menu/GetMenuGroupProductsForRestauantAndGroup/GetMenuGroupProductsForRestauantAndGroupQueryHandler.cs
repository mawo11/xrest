using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Menu;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Queries.Menu.GetMenuGroupProductsForRestauantAndGroup;

public sealed class GetMenuGroupProductsForRestauantAndGroupQueryHandler : IRequestHandler<GetMenuGroupProductsForRestauantAndGroupQuery, MenuProduct[]>
{
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;
	private readonly ILogger<GetMenuGroupProductsForRestauantAndGroupQueryHandler> _logger;

	public GetMenuGroupProductsForRestauantAndGroupQueryHandler(IReadOnlyProductRepository readOnlyProductRepository, ILogger<GetMenuGroupProductsForRestauantAndGroupQueryHandler> logger)
	{
		_readOnlyProductRepository = readOnlyProductRepository;
		_logger = logger;
	}

	public Task<MenuProduct[]> Handle(GetMenuGroupProductsForRestauantAndGroupQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("GetMenuGroupProductsForRestauantAndGroupQueryHandler: restId({request.RestaurantId}) groupId({request.GroupId})",
			request.RestaurantId,
			request.GroupId);

		var products = _readOnlyProductRepository.GetAllProducts()
								.Where(x => x.ProductGroupId == request.GroupId && x.IsAllowedOnlineForRestaurant(request.RestaurantId))
								.ToList();

		var result = products
					   .OrderBy(static x => x.DisplayName)
					   .Select(x => new MenuProduct
					   {
						   Id = x.Id,
						   Name = x.GetTranslatedDisplayName(request.Lang),
						   ImageUrl = $"{x.Id}&ts={x.ImageAudit?.Ticks ?? 0}",
						   Description = x.DescriptionTranslations?.GetTranslate(request.Lang, x.Description ?? string.Empty) ?? string.Empty,
						   Price = x.GetPriceForRestaurant(request.RestaurantId).ToString("C", CultureSettings.DefaultCultureInfo)
					   })
					   .ToArray();

		return Task.FromResult(result);
	}
}
