namespace XRest.Restaurants.Contracts.Synchro.Responses;
public class CommercialFile
{
	public int Id { get; set; }

	public string? Name { get; set; }

	public string? File { get; set; }

	public int Destination { get; set; }

	public string? Mime { get; set; }

	public DateTime Version { get; set; }

	public long Size { get; set; }

	public bool Enabled { get; set; }
}
