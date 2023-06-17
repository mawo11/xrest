namespace XRest.Orders.App.Domain;

public class CustomerOrderShortInfo
{
	public int Id { get; set; }

	public int RestaurantId { get; set; }

	public OrderStatus Status { get; set; }

	public DateTime CreateDate { get; set; }

	public decimal Amount { get; set; }

	public string? RestaurantName { get; set; }
}
