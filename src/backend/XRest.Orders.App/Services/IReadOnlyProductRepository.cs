using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IReadOnlyProductRepository
{
	IReadOnlyList<ReadOnlyProduct> GetAllProducts();

	ReadOnlyProduct? GetReadOnlyProductById(int productId);

	ReadOnlyProductGroup[] GetGroupsForRestaurant(int restaurantId);

	IReadOnlyList<ReadonlyVat> LoadAllVats();

	void LoadAllProducts();
}
