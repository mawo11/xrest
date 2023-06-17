namespace XRest.Identity.App.Domain;

public class Session
{
	public int Id { get; set; }

	public string? Token { get; set; }

	public DateTime Logon { get; set; }

	public DateTime LastActivity { get; set; }

	public DateTime? LogOff { get; set; }

	public int AccountId { get; set; }

	public static Session Invalid = new();
}
