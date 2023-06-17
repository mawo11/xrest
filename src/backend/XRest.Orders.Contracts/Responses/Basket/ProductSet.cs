namespace XRest.Orders.Contracts.Responses.Basket;
public class ProductSet
{
	public int Id { get; set; }

	public string? Title { get; set; }

	public ProductSetItem[]? Items { get; set; }

	public bool Required { get; set; }

	public int MainProductId { get; set; }

	public string? BundleLabel { get; set; }
}
