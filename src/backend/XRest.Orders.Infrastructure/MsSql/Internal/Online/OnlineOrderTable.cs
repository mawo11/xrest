
using XRest.Orders.App.Domain;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;
internal class OnlineOrderTable
{

	public int Id { get; set; }

	/// <summary>
	/// [payment_status] == 1
	/// </summary>
	public bool PayedOnline { get; set; }

	/// <summary>
	/// [create_date]
	/// </summary>
	public DateTime Created { get; set; }

	public string? Driver { get; set; }

	/// <summary>
	/// [driver_id]
	/// </summary>
	public int DriverId { get; set; }

	/// <summary>
	/// [discount]
	/// </summary>
	public decimal Discount { get; set; }

	/// <summary>
	/// [order_day]
	/// </summary>
	public int OrderDay { get; set; }

	/// <summary>
	/// [note]
	/// </summary>
	public string? Note { get; set; }

	/// <summary>
	/// [amount]
	/// </summary>
	public decimal Total { get; set; }

	public DeliveryType Type { get; set; }

	/// <summary>
	/// [payment_type_id]
	/// </summary>
	public int PaymentId { get; set; }

	/// <summary>
	/// [phone]
	/// </summary>
	public string? Phone { get; set; }

	/// <summary>
	/// restaurant_id
	/// </summary>
	public int RestaurantId { get; set; }

	/// <summary>
	/// [email]
	/// </summary>
	public string? Email { get; set; }

	/// <summary>
	/// [status_id]
	/// </summary>
	public OrderStatus Status { get; set; }

	/// <summary>
	/// [transport_amount]
	/// </summary>
	public decimal TransportAmount { get; set; }

	/// <summary>
	/// ,[term_type_id]
	/// </summary>
	public TermType TermType { get; set; }

	/// <summary>
	/// [term_type_id]
	/// </summary>
	public TimeSpan Term { get; set; }

	/// <summary>
	/// </summary>      
	public DeliveryType DeliveryType { get; set; }

	public OrderSource Source { get; set; }

	public string? City { get; set; }

	public string? Country { get; set; }

	public string? Street { get; set; }

	public string? StreetNumber { get; set; }

	public string? HouseNumber { get; set; }

	public string? PostCode { get; set; }

	public DateTime ModifyDate { get; set; }

	public string? CustomerFistname { get; set; }

	public string? CustomerLastname { get; set; }

	public string? StatusInfo { get; set; }

	public string? PaymentName { get; set; }

	public byte PaymentStatus { get; set; }

	public string? Restaurant { get; set; }

	public string? Worker { get; set; }
}