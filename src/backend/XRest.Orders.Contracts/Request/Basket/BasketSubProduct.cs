namespace XRest.Orders.Contracts.Request.Basket;

public class BasketSubProduct
{
	public int Id { get; set; }

	public BasketSubProductProductSetItem[]? ProductSets { get; set; }

	public string? Label { get; set; }
}
