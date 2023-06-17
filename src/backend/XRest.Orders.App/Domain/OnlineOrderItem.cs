namespace XRest.Orders.App.Domain;

public class OnlineOrderItem
{
	public OnlineProductProduct? Product { get; set; }

	public string? Note { get; set; }

	public bool TakeAway { get; set; }

	public decimal Total { get; set; }
}
