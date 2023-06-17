namespace XRest.Orders.App.Domain;

public class OnlineOrderHeader
{
	public OrderStatus Status { get; set; }

	public int RestaurantId { get; set; }

	public decimal Amount { get; set; }

	public string? StatusInfo { get; set; }

	public DateTime ModifyDate { get; set; }
}
