using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Services;

public interface IFavoriteAddressRepository
{
	ValueTask SaveAsync(FavoriteAddressItem favoriteAddressItem);

	ValueTask<IEnumerable<FavoriteAddressItem>> GetForAccountAsync(int accountId);

	ValueTask RemoveAsync(int accountId, int id);
}
