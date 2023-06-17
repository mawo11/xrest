using Consul;
using Microsoft.Extensions.Caching.Memory;
using System.Buffers;

namespace XRest.Consul;

public sealed class ConsulServiceDiscovery : IConsulServiceDiscovery
{
	private readonly IMemoryCache _memoryCache;
	private readonly IConsulClient _consulClient;
	private readonly ConsulClientConfiguration _consulClientConfiguration;

	public ConsulServiceDiscovery(IMemoryCache memoryCache,
		IConsulClient consulClient)
	{
		_memoryCache = memoryCache;
		_consulClient = consulClient;
		_consulClientConfiguration = ConsulClientConfigurationFactory.Get();
	}

	public async ValueTask<string?> GetAddressAsync(string serviceName)
	{
		string cacheKey = $"servicediscover_{serviceName}";

		if (_memoryCache.TryGetValue(cacheKey, out string[] serviceUrls))
		{
			return serviceUrls.ElementAtOrDefault(0);
		}

		var serviceCatalogs = await _consulClient.Catalog.Service(_consulClientConfiguration.Enviroment + serviceName);
		if (serviceCatalogs != null && serviceCatalogs.StatusCode == System.Net.HttpStatusCode.OK && serviceCatalogs.Response != null)
		{
			serviceUrls = serviceCatalogs.Response
				.Select(static x => $"http://{x.ServiceAddress}:{x.ServicePort}")
				.ToArray();
		}

		_memoryCache.Set(cacheKey, serviceUrls, TimeSpan.FromSeconds(60));

		return serviceUrls.ElementAtOrDefault(0);
	}
}
