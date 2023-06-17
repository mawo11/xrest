namespace XRest.Orders.App.Domain;

public class UndeliveredReason
{
	public int? Id { get; set; }

	public string? Reason { get; set; }

	public SourceType SourceType { get; set; }
}
