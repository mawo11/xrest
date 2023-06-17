namespace XRest.Restaurants.Contracts;

public class GetRestaurantShortInfoByIdResponse
{
	public bool Success { get; set; }

	public RestaurantShortInfo? RestaurantShortInfo { get; set; }
}
