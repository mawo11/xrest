namespace XRest.Identity.Contracts.ExternalAuthenticate.Request;

public class ExternalAuthenticateLogonRequest
{
	public string? Email { get; set; }

	public string? ExternalIdentifier { get; set; }

	public string? Firstname { get; set; }

	public string? Surname { get; set; }

	public ClientType ClientType { get; set; }

	public ExternalProviderType ExternalProvider { get; set; }
}
