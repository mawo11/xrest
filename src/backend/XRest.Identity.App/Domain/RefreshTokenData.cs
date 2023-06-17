namespace XRest.Identity.App.Domain;

public class RefreshTokenData
{
	public int Id { get; set; }

	public string Token { get; set; } = string.Empty;

	public DateTime Created { get; set; }

	public DateTime Expired { get; set; }

}
