using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.FindRestaurantByLocation;

public sealed class FindRestaurantByLocationQueryHandler : IRequestHandler<FindRestaurantByLocationQuery, RestaurantOrderingData>
{
	private readonly static RestaurantOrderingData NotFound = new();
	private readonly ILocationFinderService _locationFinderService;
	private readonly ITransportZoneFinderService _transportZoneFinderService;
	private readonly ILogger<FindRestaurantByLocationQueryHandler> _logger;
	private readonly IRestaurantCache _restaurantFactory;

	public FindRestaurantByLocationQueryHandler(ILocationFinderService locationFinderService,
		ITransportZoneFinderService transportZoneFinderService,
		ILogger<FindRestaurantByLocationQueryHandler> logger,
		IRestaurantCache restaurantFactory)
	{
		_locationFinderService = locationFinderService;
		_transportZoneFinderService = transportZoneFinderService;
		_logger = logger;
		_restaurantFactory = restaurantFactory;
	}

	public async Task<RestaurantOrderingData> Handle(FindRestaurantByLocationQuery request, CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("szukamy {city} +> {stree}", request.City, request.Street);

			int addrId = _locationFinderService.FindLocation(request.City, request.Street);
			if (addrId == LocationFinderService.LocationNotFound)
			{
				_logger.LogInformation("brak addrId");
				return NotFound;
			}

			int restaurantTransportId = _transportZoneFinderService.FindRestaurantByAddr(addrId, request.StreetNumber);
			if (restaurantTransportId == TransportZoneFinderService.NotFound)
			{
				_logger.LogInformation("brak restaurantTransportId");
				return NotFound;
			}

			_logger.LogInformation("restaurantTransportId: {restaurantTransportId}", restaurantTransportId);
			var info = _transportZoneFinderService.GetRestaurantTransportInfoById(restaurantTransportId);
			if (info == null)
			{
				_logger.LogInformation("brak info o transporcie");
				return NotFound;
			}

			_logger.LogInformation("info.RestaurantId: {RestaurantId}", info.RestaurantId);
			var rest = await _restaurantFactory.GetByIdAsync(info.RestaurantId);
			if (rest == null)
			{
				return NotFound;
			}

			if (request.IsPosCheckout && !rest.IsPosCheckout)
			{
				return NotFound;
			}

			return new RestaurantOrderingData(
				rest.Id,
				rest.Name,
				rest.Address,
				info.Id,
				rest.RealizationTime,
				rest.MinOrder == 0 ? string.Empty : rest.MinOrder.ToString("C", CultureSettings.DefaultCultureInfo),
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
			_logger.LogError("FindRestaurantByLocationResponse {city} {street} {message}", request.City, request.Street, e);
		}

		return NotFound;
	}




}
