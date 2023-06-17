using XRest.Orders.App.Commands.Basket.CreateOrder;
using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Validators;

public interface IOrderCreatorValidator
{
	ValueTask<OrderCreatorValidatorStatus> ValidateAsync(BasketData data, CreateOrderCommand createOrderCommand);
}
