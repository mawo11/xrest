using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface ITransportZonesRepository
{
	ValueTask<bool> IsNewDataAvailableAsync(DateTime auditDate);

	ValueTask<IEnumerable<RestaurantTransport>> LoadAllAsync();
}
