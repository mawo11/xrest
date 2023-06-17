using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace XRest.Consul;

public class ConsulHostedService : IHostedService
{
	private readonly ILogger<ConsulHostedService> _logger;
	private readonly ConsulClientConfiguration _consulClientConfiguration;
	private readonly IConsulClient _consulClient;
	private string? _registrationID;

	public ConsulHostedService(ILogger<ConsulHostedService> logger, IServer server)
	{
		_consulClientConfiguration = ConsulClientConfigurationFactory.Get();

		_consulClient = new ConsulClient(cfg =>
		{
			cfg.Address = new Uri(_consulClientConfiguration.Url!);
			if (!string.IsNullOrEmpty(_consulClientConfiguration.Token))
			{
				cfg.Token = _consulClientConfiguration.Token;
			}
		});

		_logger = logger;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		string appName = Assembly.GetEntryAssembly()?.GetName()?.Name?.Replace(".", "-")?.ToLower() ?? "xapp";

		_registrationID = $"{_consulClientConfiguration.Enviroment}-{appName}-{_consulClientConfiguration.AppPort}";

		var httpCheck = new AgentServiceCheck()
		{
			DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5),
			Interval = TimeSpan.FromSeconds(15),
			HTTP = $"http://{_consulClientConfiguration.AppIp}:{_consulClientConfiguration.AppPort}/health"
		};

		var registration = new AgentServiceRegistration()
		{
			ID = _registrationID,
			Name = $"{_consulClientConfiguration.Enviroment}-{appName}",
			Address = _consulClientConfiguration.AppIp,
			Port = _consulClientConfiguration.AppPort,
			Checks = new[] { httpCheck }
		};

		_logger.LogInformation("Registering in Consul");
		await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Deregistering from Consul");
		try
		{
			if (_consulClient != null)
			{
				await _consulClient.Agent.ServiceDeregister(_registrationID, cancellationToken);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Deregisteration failed");
		}
	}
}
