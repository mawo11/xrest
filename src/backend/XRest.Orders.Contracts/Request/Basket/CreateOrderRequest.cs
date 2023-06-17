namespace XRest.Orders.Contracts.Request.Basket;

public class CreateOrderRequest
{
	public string? Firstname { get; set; }

	public string? Email { get; set; }

	public string? Phone { get; set; }

	public string? Note { get; set; }

	public int PaymentId { get; set; }

	public int[]? MarketingIds { get; set; }

	public bool TermOfUse { get; set; }

	public InvoiceData? Invoice { get; set; }

	public DeliveryAddress? Delivery { get; set; }
}
