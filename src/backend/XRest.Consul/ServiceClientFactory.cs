using Consul;
using RestEase;

namespace XRest.Consul;

internal class ServiceClientFactory : IServiceClientFactory
{
	private readonly IConsulClient _consulClient;
	private readonly ConsulClientConfiguration _consulClientConfiguration;

	public ServiceClientFactory(IConsulClient consulClient)
	{
		_consulClient = consulClient;
		_consulClientConfiguration = ConsulClientConfigurationFactory.Get();
	}

	public T Create<T>(string serviceName) where T : class
	{
		//TODO: przebudowwa aby pamietac liste i odswwiezac co x sekund
		var services = _consulClient.Catalog.Service(_consulClientConfiguration.Enviroment + serviceName)
							.GetAwaiter()
							.GetResult();

		if (services.Response.Length > 0)
		{
			var instance = services.Response[0];

			var host = $"http://{instance.ServiceAddress}:{instance.ServicePort}";

			//TODO: dodac obsluge poly
			//
			return RestClient.For<T>(host);
		}

		throw new InvalidOperationException("service factory is null");
	}
}
