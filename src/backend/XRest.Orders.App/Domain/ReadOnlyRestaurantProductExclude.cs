namespace XRest.Orders.App.Domain;

public class ReadOnlyRestaurantProductExclude
{
	public TimeSpan From { get; set; }

	public TimeSpan To { get; set; }

	public byte Day { get; set; }
}
