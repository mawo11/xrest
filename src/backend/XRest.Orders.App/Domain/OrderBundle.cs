namespace XRest.Orders.App.Domain;

public class OrderBundle
{
	public int ProductId { get; set; }

	public string? Name { get; set; }

	public decimal Price { get; set; }

	public string? SubProducts { get; set; }

	public byte Printer { get; set; }
}
