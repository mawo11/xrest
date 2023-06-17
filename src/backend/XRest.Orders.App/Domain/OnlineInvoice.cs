namespace XRest.Orders.App.Domain;

public class OnlineInvoice
{
	public string? Address { get; set; }

	public string? Nip { get; set; }

	public string? Name { get; set; }

	public string? Number { get; set; }

	public int OrderId { get; set; }

	public DateTime CreateDate { get; set; }

	public int Id { get; set; }
}