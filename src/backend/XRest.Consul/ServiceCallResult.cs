namespace XRest.Consul;

public class ServiceCallResult
{
	public ServiceCallStatus Status { get; init; }


	public readonly static ServiceCallResult Ok = new()
	{
		Status = ServiceCallStatus.Ok
	};

	public readonly static ServiceCallResult Unauthorized = new()
	{
		Status = ServiceCallStatus.Unauthorized
	};

	public readonly static ServiceCallResult Error = new()
	{
		Status = ServiceCallStatus.Error
	};
}
