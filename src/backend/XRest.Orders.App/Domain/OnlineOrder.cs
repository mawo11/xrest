namespace XRest.Orders.App.Domain;

public class OnlineOrder
{

	public int Id { get; set; }

	public bool PayedOnline { get; set; }

	public DateTime Created { get; set; }

	public string? Driver { get; set; }

	public int? DriverId { get; set; }

	public decimal Discount { get; set; }

	public OnlineOrderItem[]? Products { get; set; }

	public int OrderDay { get; set; }

	public string? Note { get; set; }

	public decimal Total { get; set; }

	public int PaymentId { get; set; }

	public string? Phone { get; set; }

	public OnlineInvoice? OnlineInvoice { get; set; }

	public int RestaurantId { get; set; }

	public string? Email { get; set; }

	public OrderStatus Status { get; set; }

	public TermType TermType { get; set; }

	public string? Term { get; set; }

	public DeliveryType DeliveryType { get; set; }

	public OnlineOrderAddress? OrderAddress { get; set; }

	public OrderSource Source { get; set; }

	public DateTime Modified { get; set; }


	public static OnlineOrder NotFound = new();
}