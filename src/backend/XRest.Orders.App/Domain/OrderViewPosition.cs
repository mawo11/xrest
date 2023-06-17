namespace XRest.Orders.App.Domain;

public class OrderViewPosition
{
	public decimal BasePrice { get; set; }

	public int OrdinalNumber { get; set; }

	public int Index { get; set; }

	public string? Name { get; set; }

	public string? Note { get; set; }

	public decimal Price { get; set; }

	public string? SubProducts { get; set; }

	public string? VatName { get; set; }

	public string? VatGroup { get; set; }

	public int VatId { get; set; }

	public decimal VatValue { get; set; }

	public int? OrderProductId { get; set; }

	public string? BundleName { get; set; }

	public byte? FromSource { get; set; }

	public int ProductId { get; set; }

	public string? FiscalName { get; set; }
}
