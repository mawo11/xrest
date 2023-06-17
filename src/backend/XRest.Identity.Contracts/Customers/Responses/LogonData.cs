
using XRest.Identity.Contracts.Common;

namespace XRest.Identity.Contracts.Customers.Responses;
public class LogonData
{
	public TokenData? Token { get; set; }

	public string? Email { get; set; }

	public string? Phone { get; set; }

	public bool MustChangePassword { get; set; }

	public string? Firstname { get; set; }

	public string? Lastname { get; set; }

	public LogonDataStatus Status { get; set; }
}
