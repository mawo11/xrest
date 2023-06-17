namespace XRest.Orders.App.Domain;

public class PaymentInfo
{
	public DateTime Created { get; set; }

	public string? Type { get; set; }

	public int OrderId { get; set; }

	public string? Method { get; set; }

	public string? Statement { get; set; }

	public string? Response { get; set; }
}
