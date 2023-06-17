namespace XRest.Identity.Contracts.Common;

public class ApiOperationResultData<T>
{

	public ApiOperationResultData()
	{

	}

	public ApiOperationResultData(ApiOperationResultStatus status, T data)
	{
		Status = status;
		Data = data;
	}

	public ApiOperationResultData(ApiOperationResultStatus status)
	{
		Status = status;
		Data = default;
	}

	public ApiOperationResultStatus Status { get; set; }

	public T? Data { get; set; }
}
