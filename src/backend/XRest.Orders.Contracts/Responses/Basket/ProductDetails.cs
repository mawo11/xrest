namespace XRest.Orders.Contracts.Responses.Basket;

public class ProductDetails
{
	public string? Id { get; set; }

	public int ProductId { get; set; }

	public bool NotFound { get; set; }

	public ProductType Type { get; set; }

	public string? ImageUrl { get; set; }

	public string? Title { get; set; }

	public ProductSet[]? ProductSets { get; set; }

	public ProductBundle[]? Bundles { get; set; }

	public decimal Price { get; set; }

	public string? Note { get; set; }
}
