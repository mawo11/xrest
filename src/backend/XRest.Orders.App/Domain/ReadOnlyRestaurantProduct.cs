namespace XRest.Orders.App.Domain;

public class ReadOnlyRestaurantProduct
{
	public int Id { get; set; }

	public int RestaurantId { get; set; }

	public int ProductId { get; set; }

	public string? FiscalName { get; set; }

	public bool Hidden { get; set; }

	public bool CanChangeForPoints { get; set; }

	public DateTime AuditDate { get; set; }

	public byte DayOfWeek { get; set; }

	public decimal Price { get; set; }

	public bool QuickAccess { get; set; }

	public decimal PosPrice { get; set; }

	public string? OrderName { get; set; }

	public bool? PriceChange { get; set; }

	public IList<ReadOnlyRestaurantProductExclude>? Excludes { get; set; }

	public IList<ReadOnlyRestaurantProductExtra>? Extras { get; set; }

	public int[]? CashDeskId { get; set; }
}
