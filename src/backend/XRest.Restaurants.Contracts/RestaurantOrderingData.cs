namespace XRest.Restaurants.Contracts;

public class RestaurantOrderingData
{
	public RestaurantOrderingData()
	{
		Found = false;
	}

	public RestaurantOrderingData(int id,
		string name,
		string? address,
		int transportZoneID,
		int realizationTime,
		string? minOrder,
		bool working,
		bool isPosCheckout,
		string? alias,
		string? postCode,
		string? city,
		string? onlineFrom,
		string? onlineTo,
		bool found)
	{
		Id = id;
		Name = name;
		Address = address;
		TransportZoneID = transportZoneID;
		RealizationTime = realizationTime;
		MinOrder = minOrder;
		Working = working;
		IsPosCheckout = isPosCheckout;
		Found = found;
		Alias = alias;
		City = city;
		PostCode = postCode;
		OnlineFrom = onlineFrom;
		OnlineTo = onlineTo;
	}

	public int Id { get; }

	public string? Name { get; }

	public string? Address { get; }

	public int TransportZoneID { get; }

	public int RealizationTime { get; }

	public string? MinOrder { get; }

	public bool Working { get; }

	public bool IsPosCheckout { get; }

	public bool Found { get; }

	public string? Alias { get; }

	public string? PostCode { get; }

	public string? City { get; set; }

	public string? OnlineFrom { get; set; }

	public string? OnlineTo { get; set; }
}
