namespace XRest.Orders.App.Domain;

public class ReadOnlyRestaurantProductExtra
{
	public int RestaurantId { get; set; }

	public decimal FromOrderAmount { get; set; }

	public decimal Price { get; set; }
}
