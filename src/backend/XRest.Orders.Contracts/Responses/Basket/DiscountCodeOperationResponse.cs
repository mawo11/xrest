namespace XRest.Orders.Contracts.Responses.Basket;

public class DiscountCodeOperationResponse
{
	public DiscountCodeOperationResponse(DiscountCodeOperationStatus status)
	{
		Status = status;
	}

	public DiscountCodeOperationStatus Status { get; }
}
