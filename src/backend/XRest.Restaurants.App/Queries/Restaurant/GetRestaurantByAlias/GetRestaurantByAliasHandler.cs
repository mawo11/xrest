using XRest.Restaurants.App.Queries.FindRestaurantByLocation;
using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.Restaurant.GetRestaurantByAlias;

public sealed class GetRestaurantByAliasHandler : IRequestHandler<GetRestaurantByAliasQuery, RestaurantOrderingData>
{
	private readonly static RestaurantOrderingData NotFound = new();

	private readonly IRestaurantCache _restaurantFactory;
	private readonly ILogger<FindRestaurantByLocationQueryHandler> _logger;

	public GetRestaurantByAliasHandler(IRestaurantCache restaurantFactory, ILogger<FindRestaurantByLocationQueryHandler> logger)
	{
		_restaurantFactory = restaurantFactory;
		_logger = logger;
	}

	public async Task<RestaurantOrderingData> Handle(GetRestaurantByAliasQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var rest = await  _restaurantFactory.GetByAliasAsync(request.Alias);
			if (rest == null)
			{
				return NotFound;
			}

			return new RestaurantOrderingData(
				rest.Id,
				rest.Name,
				rest.Address,
				-1,
				rest.RealizationTime,
				rest.MinOrder == 0 ? null : rest.MinOrder.ToString("C", CultureSettings.DefaultCultureInfo),
				rest.IsWorkingOnline(DateTime.Now),
				rest.IsPosCheckout,
				rest.Alias,
				rest.PostCode,
				rest.City,
				rest.GetOnlineFrom(),
				rest.GetOnlineTo(),
				true);
		}
		catch (Exception e)
		{
			_logger.LogError("GetRestaurantByAliasHandler {alias} {message}", request.Alias, e.Message);
		}

		return NotFound;
	}
}
