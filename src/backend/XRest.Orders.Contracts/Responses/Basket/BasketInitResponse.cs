namespace XRest.Orders.Contracts.Responses.Basket;

public class BasketInitResponse
{
	public BasketInitResponse(string key)
	{
		Key = key;
	}

	public string Key { get; }
}
