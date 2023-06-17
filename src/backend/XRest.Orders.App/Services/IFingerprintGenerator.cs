using XRest.Orders.Contracts.Request.Basket;

namespace XRest.Orders.App.Services;

public interface IFingerprintGenerator
{
	string Generate(string basketKey, BasketItemSelectedProduct productDetails);
}