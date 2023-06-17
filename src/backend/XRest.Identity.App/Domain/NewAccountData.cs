namespace XRest.Identity.App.Domain;

public class NewAccountData
{
	public string? Firstname { get; set; }

	public string? Lastname { get; set; }

	public string? Email { get; set; }

	public string? Password { get; set; }

	public bool Marketing { get; set; }

	public bool Terms { get; set; }

	public ExternalProvider ExernalProvider { get; set; }

	public string? ExternalId { get; set; }
}
