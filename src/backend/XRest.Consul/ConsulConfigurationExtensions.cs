using Microsoft.Extensions.Configuration;

namespace XRest.Consul;

public static class ConsulConfigurationExtensions
{
	public static void AddConsulSource(this IConfigurationBuilder builder)
	{
		builder.Add(new ConsulConfigurationSource(ConsulClientConfigurationFactory.Get()));
	}
}
