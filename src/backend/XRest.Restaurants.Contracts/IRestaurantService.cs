using RestEase;

namespace XRest.Restaurants.Contracts;

public interface IRestaurantService
{
	[Get("/restaurant/{restaurantId}/is-working")]
	Task<RestaurantWorkingStatusResposne> IsRestaurantWorkingAsync([Path] int restaurantId);

	[Post("/restaurant/{restaurantId}/calculate-delivery-price")]
	Task<CalculateDeliveryPriceResponse> CalculateDeliveryPriceAsync([Path] int restaurantId, [Body] CalculateDeliveryPriceRequest request);

	[Get("/restaurant/{restaurantId}/ordering-information")]
	Task<RestaurantOrderInformation> GetInformationForOrderingAsync([Path] int restaurantId);
}
