namespace XRest.Restaurants.App.Domain;

public class RestaurantTransportPrice
{
	public int Id { get; set; }

	public int RestaurantTransportId { get; set; }

	public decimal DeliveryPrice { get; set; }

	public decimal FromPrice { get; set; }

	public DateTime AuditDate { get; set; }
}
