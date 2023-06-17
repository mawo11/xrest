namespace XRest.Identity.Contracts.Customers.Request;

public class ResetPasswordRequest
{
	public string? Password { get; set; }

	public string? Token { get; set; }
}
