namespace XRest.Identity.Contracts.Customers.Responses;

public class NewAccountResponse
{
	public NewAccountResponse(NewAccountStatus status)
	{
		Status = status;
	}

	public NewAccountStatus Status { get; }
}
