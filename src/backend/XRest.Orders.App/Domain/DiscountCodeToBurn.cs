namespace XRest.Orders.App.Domain;

public class DiscountCodeToBurn
{
	public int Id { get; set; }

	public string? Code { get; set; }

	public bool Burned { get; set; }

	public DateTime? Used { get; set; }

	public int? CustomerId { get; set; }

	public int OrderId { get; set; }

	public DateTime ValidUntil { get; set; }

	public decimal Value { get; set; }


	public readonly static DiscountCodeToBurn Invalid = new();
}
