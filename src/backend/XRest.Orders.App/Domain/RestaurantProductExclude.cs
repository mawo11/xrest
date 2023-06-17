namespace XRest.Orders.App.Domain;

public class RestaurantProductExclude
{
	public int Id { get; set; }

	public int RestaurantId { get; set; }

	public int ProductId { get; set; }

	public TimeSpan From { get; set; }

	public TimeSpan To { get; set; }

	public DateTime AuditDate { get; set; }

	public byte Day { get; set; }
}