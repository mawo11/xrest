namespace XRest.Restaurants.App.Domain;

public class Restaurant
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public string? Address { get; set; }

	public string? Phone { get; set; }

	public string? PostCode { get; set; }

	public string? City { get; set; }

	public string? Nip { get; set; }

	public bool Active { get; set; }

	public string? Alias { get; set; }

	public string? Email { get; set; }

	public DateTime AuditDate { get; set; }

	public int DefaultRealizationTime { get; set; }

	public int RealizationTime { get; set; }

	public string? InvoiceAddress { get; set; }

	public int DayNumber { get; set; }

	public decimal MinOrder { get; set; }

	public string? Terms { get; set; }

	public string? PaymentId { get; set; }

	public string? PaymentSecret { get; set; }

	public string? JpkStreet { get; set; }

	public string? JpkHomeNumber { get; set; }

	public string? JpkHouseNumber { get; set; }

	public bool IsPosCheckout { get; set; }

	public List<RestaurantWorkingTime> Workings { get; set; } = new List<RestaurantWorkingTime>();

	public List<RestaurantExcludeTime> Excludes { get; set; } = new List<RestaurantExcludeTime>();

	public DateTime GetMaxAuditDate()
	{
		DateTime[] dates = new DateTime[3];
		dates[0] = Workings.Any() ? Workings.Max(x => x.AuditDate) : DateTime.MinValue;
		dates[0] = Excludes.Any() ? Excludes.Max(x => x.AuditDate) : DateTime.MinValue;

		return dates.Max(x => x);
	}

	public string? GetOnlineFrom()
	{
		var workingDay = Workings.Find(x => x.Day == (byte)DateTime.Now.DayOfWeek);
		return workingDay?.OnlineFromFormatted;
	}

	public string? GetOnlineTo()
	{
		var workingDay = Workings.Find(x => x.Day == (byte)DateTime.Now.DayOfWeek);
		return workingDay?.OnlineToFormatted;
	}

	internal bool IsWorkingOnline(DateTime currentDatetime)
	{
		//TODO: do poprawy
		DateTime zeroH = new(currentDatetime.Year, currentDatetime.Month, currentDatetime.Day, 0, 0, 0);

		TimeSpan nowHour = currentDatetime - zeroH;

		var workingDay = Workings.Find(x => x.Day == (byte)currentDatetime.DayOfWeek);

		if (workingDay != null)
		{
			return workingDay.OnlineFrom <= nowHour && nowHour <= workingDay.OnlineTo && workingDay.Working;
		}

		return false;
	}

	internal bool IsWorking(DateTime currentDatetime)
	{
		//TODO: do poprawy
		DateTime zeroH = new(currentDatetime.Year, currentDatetime.Month, currentDatetime.Day, 0, 0, 0);

		TimeSpan nowHour = currentDatetime - zeroH;

		var workingDay = Workings.Find(x => x.Day == (byte)currentDatetime.DayOfWeek);

		bool exclude = Excludes.Find(x => x.From.Date <= zeroH && zeroH <= x.To.Date) != null;

		if (workingDay != null && !exclude)
		{
			return workingDay.WorkingFrom <= nowHour && nowHour <= workingDay.WorkingTo && workingDay.Working;
		}

		return false;
	}
}
