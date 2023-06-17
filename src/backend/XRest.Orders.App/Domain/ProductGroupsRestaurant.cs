namespace XRest.Orders.App.Domain;

public class ProductGroupsRestaurant
{
	public int Id { get; set; }

	public int ProductGroupId { get; set; }

	public int RestaurantId { get; set; }

	public byte Index { get; set; }
}
