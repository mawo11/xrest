namespace XRest.Orders.Contracts.Responses.Basket;
public class BasketView
{
	public BasketViewItem[] Items { get; set; } = Array.Empty<BasketViewItem>();

	public string? Total { get; set; }

	public string? Transport { get; set; }

	public bool CanSubmit { get; set; }

	public bool Empty { get; set; }

	public bool IsDelivey { get; set; }
}
