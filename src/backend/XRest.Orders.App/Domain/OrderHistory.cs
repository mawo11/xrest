namespace XRest.Orders.App.Domain;

public class OrderHistory
{
	public int Id { get; set; }

	public int OrderId { get; set; }

	public DateTime Created { get; set; }

	public string CreatedText
	{
		get
		{
			return Created.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}

	public string? Status { get; set; }

	public string? Info { get; set; }
}
