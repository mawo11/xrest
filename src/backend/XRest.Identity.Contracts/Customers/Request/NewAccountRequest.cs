namespace XRest.Identity.Contracts.Customers.Request;

public class NewAccountRequest
{
	public string? Firstname { get; set; }

	public string? Lastname { get; set; }

	public string? Email { get; set; }

	public string? Password { get; set; }

	public bool Marketing { get; set; }

	public bool Terms { get; set; }
}
