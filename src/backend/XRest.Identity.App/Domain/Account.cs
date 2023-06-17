namespace XRest.Identity.App.Domain;

public class Account
{
	public int Id { get; set; }

	public string? Email { get; set; }

	public string? Password { get; set; }

	public DateTime Created { get; set; }

	public bool IsDeleted { get; set; }

	public bool Islocked { get; set; }

	public string? LockedReason { get; set; }

	public string? Phone { get; set; }

	public string? DisplayName { get; set; }

	public int Points { get; set; }

	public bool MustChangePassword { get; set; }

	public string? Code { get; set; }

	public int MaxOrders { get; set; }

	public string? Firstname { get; set; }

	public string? Lastname { get; set; }

	public AccountStatus Status { get; set; }

	public bool TermsAccepted { get; set; }

	public bool Marketing { get; set; }

	public DateTime BirthDate { get; set; }

	public string? CardCode { get; set; }


	public static Account Invalid = new();
}
