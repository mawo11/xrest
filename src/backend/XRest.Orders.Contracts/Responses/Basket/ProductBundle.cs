namespace XRest.Orders.Contracts.Responses.Basket;

public class ProductBundle
{
	public string? Label { get; set; }

	public ProductBundleItem[]? Items { get; set; }
}
