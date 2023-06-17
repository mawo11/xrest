namespace XRest.Orders.App.Domain;

public class NewOnlineOrder
{
	public int? CustomerId { get; set; }

	public int? WorkedId { get; set; }

	public DateTime Created { get; set; }

	public DateTime Modified { get; set; }

	public DeliveryType DeliveryType { get; set; }

	public OrderSource Source { get; set; }

	public OnlineOrderAddress? OrderAddress { get; set; }

	public string? Phone { get; set; }

	public string? Note { get; set; }

	public string? Email { get; set; }

	public int PaymentId { get; set; }

	public TermType TermType { get; set; }

	public TimeSpan TermHour { get; set; }

	public int RestaurantId { get; set; }

	public int OrderDay { get; set; }

	public bool PayedOnline { get; set; }

	public OnlineInvoice? OnlineInvoice { get; set; }

	public OrderStatus Status { get; set; }

	public int TransportZoneId { get; set; }

	public List<BasketItem>? Items { get; set; }

	public decimal Discount { get; set; }

	public string? Firstname { get; set; }

	public DateTime AuditDate { get; set; }

	public bool Marketing { get; set; }

	public bool Realization { get; set; }

	public bool TermOfUse { get; set; }

	public decimal Amount { get; set; }

	public decimal TransportAmount { get; set; }

	public ReadOnlyProduct? BagProduct { get; set; }

	public int[] MarketingIds { get; set; } = Array.Empty<int>();

	public DiscountCodeToBurn? DiscountCode { get; set; }

	public BasketItemGratis? GratisProduct { get; set; }
}
