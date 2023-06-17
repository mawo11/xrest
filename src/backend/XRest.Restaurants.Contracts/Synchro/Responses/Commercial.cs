namespace XRest.Restaurants.Contracts.Synchro.Responses;
public class Commercial
{
	public int Id { get; set; }

	public int? RestaurantId { get; set; }

	public int? CashdeskId { get; set; }

	public int CommercialFileId { get; set; }

	public DateTime? ValidFrom { get; set; }

	public DateTime? ValidTo { get; set; }

	public TimeSpan? HourFrom { get; set; }

	public TimeSpan? HourTo { get; set; }

	public byte? Day { get; set; }
}
