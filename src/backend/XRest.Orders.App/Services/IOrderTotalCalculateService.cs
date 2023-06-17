using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IOrderTotalCalculateService
{
	ValueTask<decimal> CalculateAsync(BasketData basketData);
}