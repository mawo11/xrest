namespace XRest.Restaurants.App.Domain;

public class Address
{
	public int Id { get; set; }

	public int VoivodeshipId { get; set; }

	public string? County { get; set; }

	public string? Commune { get; set; }

	public string? City { get; set; }

	public string? Street { get; set; }

	public string? PostCode { get; set; }

	public string? Sector { get; set; }

	public string? Attribute { get; set; }

	public DateTime AuditDate { get; set; }
}
