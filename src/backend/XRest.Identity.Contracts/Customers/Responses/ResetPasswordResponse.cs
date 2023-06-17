namespace XRest.Identity.Contracts.Customers.Responses;

public class ResetPasswordResponse
{
	public ResetPasswordResponse(ResetPasswordStatus status)
	{
		Status = status;
	}

	public ResetPasswordStatus Status { get; }
}
