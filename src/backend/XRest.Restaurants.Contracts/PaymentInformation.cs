namespace XRest.Restaurants.Contracts;

public class PaymentInformation
{
	public PaymentInformation(bool exists, string? paymentId, string? paymentSecret)
	{
		Exists = exists;
		PaymentId = paymentId;
		PaymentSecret = paymentSecret;
	}

	public string? PaymentId { get; }

	public string? PaymentSecret { get; }

	public bool Exists { get; }
}
