namespace XRest.Orders.App.Domain;

public class ProductSetItem
{
	public int Id { get; set; }

	public int ProductSetsId { get; set; }

	public int ProductId { get; set; }

	public byte Amount { get; set; }

	public byte Order { get; set; }

	public DateTime AuditDate { get; set; }
}
