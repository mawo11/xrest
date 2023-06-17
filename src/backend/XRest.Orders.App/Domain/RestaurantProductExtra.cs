namespace XRest.Orders.App.Domain;

public class RestaurantProductExtra
{
	public int Id { get; set; }

	public int RestaurantId { get; set; }

	public int ProductId { get; set; }

	public decimal FromOrderAmount { get; set; }

	public decimal Price { get; set; }

	public DateTime AuditDate { get; set; }
}
