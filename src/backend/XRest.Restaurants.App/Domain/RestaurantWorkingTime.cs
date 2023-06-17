namespace XRest.Restaurants.App.Domain;

public class RestaurantWorkingTime
{
	public int Id { get; set; }

	public byte Day { get; set; }

	public TimeSpan WorkingFrom { get; set; }

	public TimeSpan WorkingTo { get; set; }

	public TimeSpan OnlineFrom { get; set; }

	public string OnlineFromFormatted => $"{OnlineFrom.Hours:D2}:{OnlineFrom.Minutes:D2}";

	public TimeSpan OnlineTo { get; set; }

	public string OnlineToFormatted => $"{OnlineTo.Hours:D2}:{OnlineTo.Minutes:D2}";

	public bool Working { get; set; }

	public int RestaurantId { get; set; }

	public DateTime AuditDate { get; set; }
}