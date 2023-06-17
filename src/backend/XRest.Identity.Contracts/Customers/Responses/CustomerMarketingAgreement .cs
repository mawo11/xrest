namespace XRest.Identity.Contracts.Customers.Responses;

public class CustomerMarketingAgreement
{
	public int Id { get; set; }

	public bool Checked { get; set; }

	public string? Content { get; set; }
}
