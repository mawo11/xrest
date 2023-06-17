namespace XRest.Orders.App.Domain;

public class ReadOnlyProductGroup
{
	public int Id { get; set; }

	public string? Name { get; set; }

	public string? NameTranslations { get; set; }

	public bool System { get; set; }

	public bool Package { get; set; }

	public string? BackgroundColor { get; set; }

	public string? TextColor { get; set; }

	public ProductGroupType Type { get; set; }

	public DateTime? ImageAudit { get; set; }
}
