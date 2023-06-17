namespace XRest.Orders.App.Domain;

public class ProductSetProduct
{
	public int Id { get; set; }

	public int ProductId { get; set; }

	public int ProductSetId { get; set; }

	public byte Order { get; set; }
}