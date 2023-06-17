namespace XRest.Orders.App.Domain;

public class ReadOnlyProductSetItem
{
	public int Id { get; set; }

	public int ProductId { get; set; }

	public ReadOnlyProduct? Product { get; set; }

	public byte Amount { get; set; }

	public byte Order { get; set; }
}
