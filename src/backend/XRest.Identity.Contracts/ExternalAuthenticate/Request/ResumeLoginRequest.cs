namespace XRest.Identity.Contracts.ExternalAuthenticate.Request;

public class ResumeLoginRequest
{
	public string? Data { get; set; }

	public bool CreateNewAccount { get; set; }
}
