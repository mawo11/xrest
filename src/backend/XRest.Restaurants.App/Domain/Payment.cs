namespace XRest.Restaurants.App.Domain;

public class Payment
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public bool Ordering { get; set; }

	public bool CallCenter { get; set; }

	public string NameTranslations { get; set; } = string.Empty;
}
