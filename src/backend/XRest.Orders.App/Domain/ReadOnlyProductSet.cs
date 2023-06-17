namespace XRest.Orders.App.Domain;

public class ReadOnlyProductSet
{
	public int Id { get; set; }

	public ProductSetType Type { get; set; }

	public string? DisplayName { get; set; }

	public string? DisplayNameTranslations { get; set; }

	public string? Name { get; set; }

	public ReadOnlyProductSetItem[]? Items { get; set; }

	//TODO: do usuniecia po przejsciu  POS na nowe 
	public int RawId { get; set; }
}
