namespace XRest.Restaurants.Contracts;

public class RestaurantOrderInformation
{
	public decimal MinOrder { get; set; }

	public int OrderDay { get; set; }

	public bool Success { get; set; }
}
