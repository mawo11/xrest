using XRest.Orders.App.Commands.Basket.CreateOrder;
using XRest.Orders.App.Domain;
using XRest.Restaurants.Contracts;

namespace XRest.Orders.App.Validators;

internal class RestaurantValidator : IOrderCreatorValidator
{
	private readonly IRestaurantService _restaurantService;

	public RestaurantValidator(IRestaurantService restaurantService)
	{
		_restaurantService = restaurantService;
	}

	public async ValueTask<OrderCreatorValidatorStatus> ValidateAsync(BasketData data, CreateOrderCommand createOrderCommand)
	{
		var result = await _restaurantService.IsRestaurantWorkingAsync(data.RestaurantId);
		if (result == null)
		{
			return OrderCreatorValidatorStatus.InvalidRest;
		}

		return result.IsWorking ?
			OrderCreatorValidatorStatus.Ok :
			OrderCreatorValidatorStatus.InvalidRest;
	}
}