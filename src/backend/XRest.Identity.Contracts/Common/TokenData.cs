using System.Text.Json.Serialization;

namespace XRest.Identity.Contracts.Common;

public class TokenData
{
	[JsonPropertyName("access_token")]
	public string? AccessToken { get; set; }

	[JsonPropertyName("refresh_token")]
	public string? RefreshToken { get; set; }

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; set; }

	public static TokenData InvalidToken = new();
}
