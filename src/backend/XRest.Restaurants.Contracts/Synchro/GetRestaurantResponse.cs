namespace XRest.Restaurants.Contracts;

public class GetRestaurantResponse
{
	public GetRestaurantResponse(Restaurant[] restaurants)
	{
		Restaurants = restaurants;
	}

	public Restaurant[] Restaurants { get; }
}