namespace XRest.Identity.Contracts.Customers.Responses;
public class CustomerPointsHistory
{
	public IEnumerable<CustomerPoint>? Items { get; set; }

	public int Total { get; set; }
}
