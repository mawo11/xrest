namespace XRest.Orders.Contracts.Request.Basket;

public class BasketItemSelectedProduct
{
	public string? Id { get; set; }

	public int ProductId { get; set; }

	public string? Note { get; set; }

	public BasketSubProduct[]? SubProducts { get; set; }
}
