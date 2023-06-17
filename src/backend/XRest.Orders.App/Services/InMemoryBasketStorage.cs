using XRest.Orders.App.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace XRest.Orders.App.Services;

public class InMemoryBasketStorage : IBasketStorage
{
	private readonly int MaxLiveTime = 30;
	private readonly int MaxBaskets = 1000;

	private readonly IMemoryCache _memoryCache;
	private int _currentCount;

	public InMemoryBasketStorage(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
		_currentCount = 0;
	}

	public void Add(BasketData basketData)
	{
		if (_currentCount >= MaxBaskets)
		{
			return;
		}

		_currentCount++;
		_memoryCache.Set(basketData.BasketKey, basketData, TimeSpan.FromMinutes(MaxLiveTime));
	}

	public BasketData? GetByKey(string basketKey)
	{
		_memoryCache.TryGetValue(basketKey, out BasketData basketData);

		return basketData;
	}

	public void Remove(string basketKey)
	{
		_memoryCache.Remove(basketKey);
	}
}
