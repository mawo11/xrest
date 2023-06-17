namespace XRest.Restaurants.Contracts.Synchro.Responses;
public class Message
{
	public int Id { get; set; }

	public MessageType Type { get; set; }

	public int? ObjectId { get; set; }

	public string? Text { get; set; }

	public int RestaurantId { get; set; }

	public DateTime? ValidFrom { get; set; }

	public DateTime? ValidTo { get; set; }
}
