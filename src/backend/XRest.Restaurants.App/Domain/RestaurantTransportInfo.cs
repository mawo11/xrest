namespace XRest.Restaurants.App.Domain;

public class RestaurantTransportInfo
{
	public RestaurantTransportInfo(int id, string name, string? plu, int restaurantId, RestaurantTransportPrice[] prices)
	{
		Id = id;
		Name = name;
		Plu = plu;
		RestaurantId = restaurantId;
		Prices = prices;
	}

	public int Id { get; }

	public string Name { get; }

	public string? Plu { get; }

	public int RestaurantId { get; }

	public RestaurantTransportPrice[] Prices { get; }
}
