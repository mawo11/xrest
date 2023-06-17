namespace XRest.Shared.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;
}
