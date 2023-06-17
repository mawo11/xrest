namespace XRest.Orders.Contracts.Responses.FavoriteAddresses;

public class FavoriteAddress
{
	public int Id { get; set; }

	public string? AddressCity { get; set; }

	public string? AddressStreet { get; set; }

	public string? AddressStreetNumber { get; set; }

	public string? AddressHouseNumber { get; set; }

	public bool Default { get; set; }
}
