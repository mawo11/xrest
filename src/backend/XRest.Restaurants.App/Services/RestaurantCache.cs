using Microsoft.Extensions.Caching.Memory;
using XRest.Restaurants.App.Domain;

namespace XRest.Restaurants.App.Services;

public sealed class RestaurantCache : IRestaurantCache
{
	private readonly IMemoryCache _memoryCache;
	private readonly IRestaurantRepository _restaurantRepository;

	public RestaurantCache(IMemoryCache memoryCache, IRestaurantRepository restaurantRepository)
	{
		_memoryCache = memoryCache;
		_restaurantRepository = restaurantRepository;
	}

	public async ValueTask<Restaurant?> GetByAliasAsync(string alias)
	{
		var items = await GetAllRestaurantsAsync();

		return items.FirstOrDefault(x => !string.IsNullOrEmpty(x.Alias) && x.Alias.ToLower() == alias.ToLower());
	}

	public async ValueTask<Restaurant?> GetByIdAsync(int id)
	{
		var items = await GetAllRestaurantsAsync();
		return items.FirstOrDefault(x => x.Id == id);
	}
	
	public async ValueTask<Restaurant[]> GetAllRestaurantsAsync()
	{
		return await _memoryCache.GetOrCreateAsync<Restaurant[]>("rest_mem", async (cache) =>
		{
			var items = await _restaurantRepository.LoadAllRestaurantsAsync();
			cache.SlidingExpiration = TimeSpan.FromMinutes(10);


			return items;
		});
	}


}
