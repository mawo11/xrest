namespace XRest.Restaurants.Contracts.Synchro.Responses;

public class PosRestaurantInfo
{
	public CashDesk[]? CashDesks { get; set; }

	public string? City { get; set; }

	public string? Address { get; set; }

	public string? PostCode { get; set; }

	public int Id { get; set; }

	public string? InvoiceAddress { get; set; }

	public string? Name { get; set; }

	public string? Nip { get; set; }

	public RestaurantStartSequence? StartSequence { get; set; }
}
