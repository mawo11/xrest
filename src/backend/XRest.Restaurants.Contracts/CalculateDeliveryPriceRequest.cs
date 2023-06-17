namespace XRest.Restaurants.Contracts;

public class CalculateDeliveryPriceRequest
{
	public decimal OrderTotal { get; set; }

	public int TransportZoneId { get; set; }
}
