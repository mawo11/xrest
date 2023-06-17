namespace XRest.Identity.Contracts.ExternalAuthenticate.Responses;

public class ExternalLoginResponse
{
	public bool Success { get; init; }

	public string? Url { get; init; }
}
