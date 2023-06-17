using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface ILocationFinderService
{
	string[] FindCities(string cityName);

	string[] FindStreets(string cityName, string streetName);

	int FindLocation(string cityName, string streetName);

	void LoadData(IEnumerable<Address> addresses);
}