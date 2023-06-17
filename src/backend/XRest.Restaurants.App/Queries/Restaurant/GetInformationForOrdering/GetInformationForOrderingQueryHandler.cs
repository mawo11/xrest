using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.Restaurant.GetInformationForOrdering;

public sealed class GetInformationForOrderingQueryHandler : IRequestHandler<GetInformationForOrderingQuery, RestaurantOrderInformation>
{
	private readonly static RestaurantOrderInformation NotWorking = new();

	private readonly IRestaurantRepository _restaurantRepository;
	private readonly ILogger<GetInformationForOrderingQueryHandler> _logger;

	public GetInformationForOrderingQueryHandler(IRestaurantRepository restaurantRepository, ILogger<GetInformationForOrderingQueryHandler> logger)
	{
		_restaurantRepository = restaurantRepository;
		_logger = logger;
	}

	public async Task<RestaurantOrderInformation> Handle(GetInformationForOrderingQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetRestaurantWorkingStatusQueryHandler => restId: {rest}", request.RestaurantId);
		var rest = await _restaurantRepository.GetRestaurantOrderInformationByIdAsync(request.RestaurantId);
		if (rest == null)
		{
			return NotWorking;
		}

		var result = new RestaurantOrderInformation
		{
			Success = true,
			MinOrder = rest.MinOrder,
			OrderDay = rest.OrderDay
		};

		return result;
	}
}
