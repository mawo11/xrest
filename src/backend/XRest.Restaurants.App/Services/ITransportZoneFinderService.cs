using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface ITransportZoneFinderService
{
	void LoadData(IEnumerable<RestaurantTransport> items);

	RestaurantTransportInfo? GetRestaurantTransportInfoById(int id);

	int FindRestaurantByAddr(int addrid, string streetNumber);
}
