namespace XRest.Orders.App.Domain;

public class ReadOnlyProductBundle
{
	public int Id { get; set; }

	public int MainProductId { get; set; }

	public int ProductId { get; set; }

	public ReadOnlyProduct? Product { get; set; }

	public decimal? Price { get; set; }

	public bool Default { get; set; }

	public string? Label { get; set; }

	public string? LabelTranslations { get; set; }
}
