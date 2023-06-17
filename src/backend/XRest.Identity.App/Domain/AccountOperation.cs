namespace XRest.Identity.App.Domain;

public class AccountOperation
{
	public string? Token { get; set; }

	public DateTime Created { get; set; }

	public int AccountId { get; set; }

	public AccountOperationType OperationType { get; set; }


	public static AccountOperation Invalid = new();
}
