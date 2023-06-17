namespace XRest.Restaurants.Contracts.Common;

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

	public T? Data { get; set; }
}
