namespace XRest.Restaurants.App.Domain;

public class RestaurantExcludeTime
{
	public int Id { get; set; }

	public int RestaurantId { get; set; }

	public DateTime From { get; set; }

	public DateTime To { get; set; }

	public DateTime AuditDate { get; set; }
}
