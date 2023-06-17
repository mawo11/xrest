using Microsoft.Extensions.Configuration;

namespace XRest.Consul;

public class ConsulConfigurationSource : IConfigurationSource
{
	private readonly ConsulClientConfiguration _consulConfigurationClient;

	public ConsulConfigurationSource(ConsulClientConfiguration consulConfigurationClient)
	{
		_consulConfigurationClient = consulConfigurationClient;
	}

	public IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		return ConsulConfigurationProvider.Create(_consulConfigurationClient);
	}
}
