namespace XRest.Orders.Contracts.Responses.Payments;

public class OrderPaymentStatus
{
	public OrderPaymentStatus(PaymentStatus status)
	{

		Status = status;
	}

	public PaymentStatus Status { get; }
}
