using XRest.Restaurants.App.Domain;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace XRest.Restaurants.App.Services;

public class TransportZoneFinderService : ITransportZoneFinderService
{
	public const int NotFound = -1;

	private readonly List<RestaurantTransportZoneItem> _restaurantTransportZoneItems;
	private readonly List<RestaurantTransportInfo> _restaurantTransports;
	private readonly ILogger<TransportZoneFinderService> _logger;

	public TransportZoneFinderService(ILogger<TransportZoneFinderService> logger)
	{
		_logger = logger;
		_restaurantTransports = new List<RestaurantTransportInfo>();
		_restaurantTransportZoneItems = new List<RestaurantTransportZoneItem>();
	}

	public int FindRestaurantByAddr(int addrId, string streetNumber)
	{
		//_logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(_restaurantTransportZoneItems));
		_logger.LogInformation("szukamy w " + _restaurantTransportZoneItems.Count + " addri: " + addrId);
		string resultString = Regex.Match(streetNumber, @"\d+").Value;
		if (int.TryParse(resultString, out int tempStreetNumber))
		{
			_logger.LogInformation("przeszlo par" + tempStreetNumber);
			bool even = tempStreetNumber % 2 == 0;

			foreach (var restaurantTransportZoneItem in _restaurantTransportZoneItems)
			{
				if (restaurantTransportZoneItem.AddressId != addrId) continue;

				int numberFrom;
				int numberTo;

				if (even)
				{
					numberFrom = restaurantTransportZoneItem.EvenFrom;
					numberTo = restaurantTransportZoneItem.EvenTo;
				}
				else
				{
					numberFrom = restaurantTransportZoneItem.OddFrom;
					numberTo = restaurantTransportZoneItem.OddTo;
				}
				
				if (numberTo == 0 && numberFrom == 0)
				{
					return restaurantTransportZoneItem.RestaurantTransportId;
				}

				if ((numberFrom == 0 || numberFrom <= tempStreetNumber) &&
				   (numberTo == 0 || tempStreetNumber <= numberTo))
				{
					return restaurantTransportZoneItem.RestaurantTransportId;
				}
			}
		}

		return NotFound;
	}

	public RestaurantTransportInfo? GetRestaurantTransportInfoById(int id) => _restaurantTransports.Find(x => x.Id == id);

	public void LoadData(IEnumerable<RestaurantTransport> items)
	{
		_restaurantTransports.Clear();
		_restaurantTransportZoneItems.Clear();

		foreach (var restaurantTransport in items)
		{
			if (restaurantTransport.Zones == null || restaurantTransport.Zones.Length == 0)
			{
				_logger.LogError("restaurantTransport.id: {restaurantTransportId} brak stref", restaurantTransport.Id);
				continue;
			}


			_restaurantTransports.Add(new RestaurantTransportInfo(
				restaurantTransport.Id,
				restaurantTransport.Name!,
				restaurantTransport.Plu,
				restaurantTransport.RestaurantId,
				restaurantTransport.Prices!));

			foreach (var zone in restaurantTransport.Zones)
			{
				_restaurantTransportZoneItems.Add(new RestaurantTransportZoneItem
				{
					AddressId = zone.AddressId,
					RestaurantTransportId = zone.RestaurantTransportId,
					EvenFrom = zone.EvenFrom.ConvertToInt(),
					EvenTo = zone.EvenTo.ConvertToInt(),
					OddFrom = zone.OddFrom.ConvertToInt(),
					OddTo = zone.OddTo.ConvertToInt(),
					NumberFrom = zone.NumberFrom.ConvertToInt(),
					NumberTo = zone.NumberTo.ConvertToInt(),
				});
			}
		}
	}

	private class RestaurantTransportZoneItem
	{
		public int AddressId { get; set; }

		public int EvenFrom { get; set; }

		public int EvenTo { get; set; }

		public int OddFrom { get; set; }

		public int OddTo { get; set; }

		public int NumberFrom { get; set; }

		public int NumberTo { get; set; }

		public int RestaurantTransportId { get; set; }
	}
}
