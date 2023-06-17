namespace XRest.Orders.App.Domain;

public class RegisterPaymentResult
{
	public bool Success { get; set; }

	public string? Token { get; set; }

	public string? Response { get; set; }
}
