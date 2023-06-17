using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantWorkingStatus;
public sealed class GetRestaurantWorkingStatusQueryHandler : IRequestHandler<GetRestaurantWorkingStatusQuery, RestaurantWorkingStatusResposne>
{
	private readonly static RestaurantWorkingStatusResposne NotWorking = new RestaurantWorkingStatusResposne();

	private readonly IRestaurantCache _restaurantFactory;
	private readonly ILogger<GetRestaurantWorkingStatusQueryHandler> _logger;

	public GetRestaurantWorkingStatusQueryHandler(IRestaurantCache restaurantFactory, ILogger<GetRestaurantWorkingStatusQueryHandler> logger)
	{
		_restaurantFactory = restaurantFactory;
		_logger = logger;
	}

	public async Task<RestaurantWorkingStatusResposne> Handle(GetRestaurantWorkingStatusQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetRestaurantWorkingStatusQueryHandler => restId: {RestaurantId}", request.RestaurantId);
		var rest = await _restaurantFactory.GetByIdAsync(request.RestaurantId);
		if (rest == null)
		{
			return NotWorking;
		}

		var result = new RestaurantWorkingStatusResposne
		{
			IsWorking = rest.IsWorkingOnline(DateTime.Now)
		};

		return result;
	}
}
