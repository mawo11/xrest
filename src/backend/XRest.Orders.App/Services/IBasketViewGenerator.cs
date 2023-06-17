using XRest.Orders.App.Domain;
using XRest.Orders.Contracts.Responses.Basket;

namespace XRest.Orders.App.Services;

public interface IBasketViewGenerator
{
	ValueTask<BasketView> GenerateAsync(BasketData basketData, string? lang);
}