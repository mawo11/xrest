namespace XRest.Orders.Contracts.Common;

public class Result
{
	public readonly static Result SuccessResult = new(true);
	public readonly static Result Fail = new(false);

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
