namespace XRest.Orders.Contracts.Responses.Basket;

public class ProductSetItem
{
	public int Id { get; set; }

	public string? Title { get; set; }

	public bool Selected { get; set; }

	public int ProductId { get; set; }
}
