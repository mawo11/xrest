namespace XRest.Orders.Contracts.Responses.Customers;

public class MyOrder
{
	public int Id { get; set; }

	public string? Status { get; set; }

	public string? CreatedDate { get; set; }

	public string? RestaurantName { get; set; }

	public string? Amount { get; set; }
}
