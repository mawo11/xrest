namespace XRest.Restaurants.Contracts.Common;

public class ApiOperationResult
{
	public ApiOperationResult()
	{
	}

	public ApiOperationResult(ApiIOperationStatus status)
	{
		Status = status;
	}

	public ApiIOperationStatus Status { get; }
}
