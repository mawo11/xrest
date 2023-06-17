namespace XRest.Orders.Contracts.Common;

public class ApiOperationResultData<T>
{
	public ApiOperationResultData(ApiIOperationStatus status, T data)
	{
		Status = status;
		Data = data;
	}

	public ApiIOperationStatus Status { get; }

	public T Data { get; }
}
