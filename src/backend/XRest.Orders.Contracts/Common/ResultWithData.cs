namespace XRest.Orders.Contracts.Common;

public class ResultWithData<T> : Result
{
	public ResultWithData()
	{

	}

	public ResultWithData(bool success = true)
	{
		Success = success;
	}

	public ResultWithData(T data) : base(true)
	{
		Data = data;
	}

	private ResultWithData(T? data, bool success) : base(success)
	{
		Data = data;
	}

	public T? Data { get; set; }

	public static ResultWithData<T> Ok(T data) => new(data, true);

	public static ResultWithData<T> Error() => new(default, false);
}
