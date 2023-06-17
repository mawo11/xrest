namespace XRest.Orders.App.Domain;

public class OnlineProductProduct
{
	public int Id { get; set; }

	public string? Name { get; set; }

	public ProductType ProductType { get; set; }

	public decimal Price { get; set; }

	public int ProductGroupId { get; set; }

	public OrderBundle[]? Bundles { get; set; }

	public string? SubProducts { get; set; }

	public OnlineProductPackage? Package { get; set; }
}
