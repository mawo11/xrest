namespace XRest.Restaurants.Contracts.Synchro.Responses;

public class SynchronizationVersion
{
	public int Account { get; set; } = 1;

	public int Products { get; set; } = 1;

	public int Restaurant { get; set; } = 1;

	public int Discounts { get; set; } = 1;

	public int Cards { get; set; } = 1;

	public int UndeliveredReason { get; set; } = 1;

	public int Messages { get; set; } = 1;

	public int Payments { get; set; } = 1;

	public int Commercials { get; set; } = 1;
}
