using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace XRest.Consul;

public static class ConsulServiceExtensions
{
	public static IServiceCollection AddConsulAgent(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddHostedService<ConsulHostedService>();
		return serviceCollection;
	}

	public static IServiceCollection AddConsul(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IServiceClientFactory, ServiceClientFactory>();
		serviceCollection.AddSingleton<IConsulServiceDiscovery, ConsulServiceDiscovery>();

		serviceCollection.AddSingleton<IConsulClient>(x =>
		{
			var readConsulConfigurationClient = ConsulClientConfigurationFactory.Get();

			return new ConsulClient(cfg =>
			{
				cfg.Address = new Uri(readConsulConfigurationClient.Url!);
				string token = readConsulConfigurationClient.Token!;

				if (!string.IsNullOrEmpty(token))
				{
					cfg.Token = token;
				}
			});

		});

		return serviceCollection;
	}

	public static IServiceCollection AddServiceClient<T>(this IServiceCollection serviceCollection, string serviceName) where T : class
	{
		serviceCollection.AddTransient<T>(x =>
		{
			var factory = x.GetService<IServiceClientFactory>();
			return factory == null ? throw new InvalidOperationException("service factory is null") : factory.Create<T>(serviceName);
		});

		return serviceCollection;
	}
}
