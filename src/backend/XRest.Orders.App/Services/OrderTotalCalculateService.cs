using XRest.Orders.App.Domain;
using XRest.Restaurants.Contracts;

namespace XRest.Orders.App.Services;

public class OrderTotalCalculateService : IOrderTotalCalculateService
{
	private readonly IRestaurantService _restaurantService;

	public OrderTotalCalculateService(IRestaurantService restaurantService)
	{
		_restaurantService = restaurantService;
	}

	public async ValueTask<decimal> CalculateAsync(BasketData basketData)
	{
		decimal total = 0;
		foreach (var item in basketData.Items)
		{
			total += item.CalculateTotalPrice();
		}

		if (basketData.GratisProduct != null)
		{
			total += basketData.GratisProduct.CalculateTotalPrice();
		}

		basketData.DeliveryPrice = 0;
		if (basketData.Type == DeliveryType.Delivery)
		{
			var result = await _restaurantService.CalculateDeliveryPriceAsync(
				basketData.RestaurantId,
				new CalculateDeliveryPriceRequest
				{
					OrderTotal = total,
					TransportZoneId = basketData.TransportZoneId,
				});

			basketData.DeliveryPrice = result?.DeliveryPrice ?? 0;

		}

		total += basketData.BagProduct?.Price ?? 0;

		basketData.TotalWithoutDelivery = total;

		total += basketData.DeliveryPrice;
		basketData.Total = total;
		return total;
	}
}
