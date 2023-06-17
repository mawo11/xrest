using XRest.Orders.Contracts.Responses.Basket;

namespace XRest.Orders.App.Services;

public interface IMenuProductDetailsCreator
{
	ProductDetails? CreateFromProduct(int productId, string? lang = "pl");

	ProductDetails? CreateFromProduct(Domain.ReadOnlyProduct product, string? lang = "pl");
}