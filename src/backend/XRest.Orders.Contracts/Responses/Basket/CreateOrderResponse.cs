namespace XRest.Orders.Contracts.Responses.Basket;

public class CreateOrderResponse
{
	public CreateOrderResponse(CreateOrderStatus status, string? data, int orderId)
	{
		OrderId = orderId;
		Status = status;
		Data = data;
	}

	public CreateOrderStatus Status { get; }

	public string? Data { get; }

	public int OrderId { get; }
}
