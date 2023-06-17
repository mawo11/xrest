using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public interface IRestaurantRepository
{
	ValueTask<Restaurant[]> LoadAllRestaurantsAsync();

	ValueTask<RestaurantOrderDayInformation?> GetRestaurantOrderInformationByIdAsync(int restaurantId);
}
