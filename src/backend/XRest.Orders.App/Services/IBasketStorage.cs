using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IBasketStorage
{
	BasketData? GetByKey(string basketKey);

	void Remove(string basketKey);

	void Add(BasketData basketData);
}
