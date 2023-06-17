using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface IAddressRepository
{
	ValueTask<IEnumerable<Address>> LoadAllAdressesAsync();

	ValueTask<bool> IsNewDataAvailableAsync(DateTime auditDate);
}
