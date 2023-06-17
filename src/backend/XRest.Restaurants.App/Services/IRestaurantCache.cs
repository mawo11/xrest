using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface IRestaurantCache
{
	ValueTask<Restaurant?> GetByIdAsync(int id);

	ValueTask<Restaurant?> GetByAliasAsync(string alias);

	ValueTask<Restaurant[]> GetAllRestaurantsAsync();
}