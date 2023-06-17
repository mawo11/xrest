namespace XRest.Identity.Contracts.Common;

public class ApiOperationResult
{
	public ApiOperationResult(ApiOperationResultStatus status)
	{
		Status = status;
	}

	public ApiOperationResultStatus Status { get; }
}
