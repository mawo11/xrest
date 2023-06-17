namespace XRest.Orders.Contracts.Responses.Basket;

public class ProductBundleItem
{
	public int ProductId { get; set; }

	public string? Title { get; set; }

	public bool Selected { get; set; }

	public decimal Price { get; set; }
}
