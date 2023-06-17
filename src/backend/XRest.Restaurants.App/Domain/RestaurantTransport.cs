namespace XRest.Restaurants.App.Domain;

public class RestaurantTransport
{
	public int Id { get; set; }

	public string? Name { get; set; }

	public string? Plu { get; set; }

	public int RestaurantId { get; set; }

	public RestaurantTransportPrice[]? Prices { get; set; }

	public RestaurantTransportZone[]? Zones { get; set; }

	public DateTime AuditDate { get; set; }
}
