namespace XRest.Orders.App.Domain;

public class OrderView
{
	public int Id { get; set; }

	public string? Restaurant { get; set; }

	public int RestaurantId { get; set; }

	public string? CreateDateText { get; set; }

	public string? ModifyDateText { get; set; }

	public string? Customer { get; set; }

	public string? Worker { get; set; }

	public decimal Amount { get; set; }

	public string? Phone { get; set; }

	public string? Status { get; set; }

	public string? Driver { get; set; }

	public string? AddressCity { get; set; }

	public string? AddressStreet { get; set; }

	public string? AddressStreetNumber { get; set; }

	public string? AddressHouseNumber { get; set; }

	public string? AddressPostcode { get; set; }

	public string? Payment { get; set; }

	public byte PaymentStatus { get; set; }

	public OrderViewPosition[]? Positions { get; set; }

	public string? PaymentStatusText { get; set; }

	public OrderHistory[]? History { get; set; }

	public string? TermType { get; set; }

	public byte TermTypeId { get; set; }

	public string? TermTime { get; set; }

	public bool Invoice { get; set; }

	public string? InvoiceNip { get; set; }

	public string? InvoiceAddress { get; set; }

	public string? InvoiceName { get; set; }

	public string? InvoiceNumber { get; set; }

	public string? Firstname { get; set; }

	public string? Lastname { get; set; }

	public string? Email { get; set; }

	public string? Note { get; set; }

	public byte StatusId { get; set; }

	public int? DriverId { get; set; }

	public DateTime ModifyDate { get; set; }

	public string? StatusInfo { get; set; }

	public byte SourceId { get; set; }

	public string? Source { get; set; }
}
