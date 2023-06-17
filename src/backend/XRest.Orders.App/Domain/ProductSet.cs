namespace XRest.Orders.App.Domain;

public class ProductSet
{
	public int Id { get; set; }

	public ProductSetType Type { get; set; }

	public string? DisplayName { get; set; }

	public string? DisplayNameTranslations { get; set; }

	public string? Name { get; set; }

	public DateTime AuditDate { get; set; }

	public ProductSetItem[]? Items { get; set; }
}

