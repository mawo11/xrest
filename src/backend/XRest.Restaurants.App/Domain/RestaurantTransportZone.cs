namespace XRest.Restaurants.App.Domain;

public class RestaurantTransportZone
{
	public int Id { get; set; }

	public int AddressId { get; set; }

	public string? EvenFrom { get; set; }

	public string? EvenTo { get; set; }

	public string? OddFrom { get; set; }

	public string? OddTo { get; set; }

	public string? NumberFrom { get; set; }

	public string? NumberTo { get; set; }

	public int RestaurantTransportId { get; set; }

	public DateTime AuditDate { get; set; }
}
