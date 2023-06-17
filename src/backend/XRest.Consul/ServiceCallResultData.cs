namespace XRest.Consul;

public class ServiceCallResultData<T> : ServiceCallResult
{
	public T? Data { get; init; }

	public readonly static ServiceCallResultData<T> RdUnauthorized = new()
	{
		Status = ServiceCallStatus.Unauthorized
	};

	public readonly static ServiceCallResultData<T> RdError = new()
	{
		Status = ServiceCallStatus.Error
	};
}
