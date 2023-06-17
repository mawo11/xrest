namespace XRest.Identity.Contracts.Customers.Responses;

public class ActivateAccountResponse
{
	public ActivateAccountResponse(ActivateAccountResponseStatus status)
	{
		Status = status;
	}

	public ActivateAccountResponseStatus Status { get; }
}
