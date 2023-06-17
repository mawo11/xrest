namespace XRest.Orders.App.Domain;

public class OrderPaymentInfo
{
	public OrderStatus Status { get; set; }

	public string? PaymentInfo { get; set; }
}
