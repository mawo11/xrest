namespace XRest.Orders.App.Domain;

public class ReadOnlyRestaurantContext
{
	public int RestaurantId { get; set; }

	public ReadOnlyRestaurantProduct? Data { get; set; }

}
