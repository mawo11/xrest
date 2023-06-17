namespace XRest.Restaurants.Contracts.Common;

public class Result
{
	public static Result SuccessResult = new Result(true);

	public Result()
	{

	}

	public Result(bool succes)
	{
		Success = succes;
	}

	public Result(bool success, int code)
	{
		Code = code;
		Success = success;
	}

	public bool Success { get; set; }

	public int Code { get; set; }
}
