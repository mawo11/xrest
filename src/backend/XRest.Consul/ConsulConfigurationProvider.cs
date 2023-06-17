using Consul;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace XRest.Consul;

public class ConsulConfigurationProvider : ConfigurationProvider, IDisposable
{
	private static ConsulConfigurationProvider? consulConfigurationProvider;

	private readonly ConsulClientConfiguration _consulClientConfiguration;
	private readonly IConsulClient _consulClient;
	private CancellationTokenSource? _cancellationTokenSource;
	private ulong _consulConfigurationIndex;
	private Task? _pollTask;


	private ConsulConfigurationProvider(ConsulClientConfiguration consulConfigurationClient)
	{
		_consulClientConfiguration = consulConfigurationClient;
		if (_consulClientConfiguration == null || string.IsNullOrEmpty(_consulClientConfiguration.Url))
		{
			throw new ArgumentOutOfRangeException(nameof(consulConfigurationClient));
		}

		_consulClient = new ConsulClient(cfg =>
		{
			cfg.Address = new Uri(_consulClientConfiguration.Url);
			if (!string.IsNullOrEmpty(_consulClientConfiguration.Token))
			{
				cfg.Token = _consulClientConfiguration.Token;
			}
		});

		_cancellationTokenSource = new CancellationTokenSource();
	}
	~ConsulConfigurationProvider()
	{
		Dispose(false);
	}

	public override void Load()
	{
		if (_pollTask != null)
		{
			return;
		}

		Data = RetrieveData().GetAwaiter().GetResult();

		if (_consulClientConfiguration.ReloadOnChange)
		{
			_pollTask = Task.Run(async () => await WaitingForChange(_cancellationTokenSource!.Token), _cancellationTokenSource!.Token);
		}
	}

	private async Task WaitingForChange(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				var data = await RetrieveData(true);
				if (data != null && data.Count > 0)
				{
					Data = data;
					OnReload();
				}
			}
			catch
			{
				await Task.Delay(_consulClientConfiguration.Wait!.Value, cancellationToken);
			}
		}
	}

	public void RegisterOnStopped(IHostApplicationLifetime lifetime)
	{
		lifetime.ApplicationStopping.Register(Disable);
	}

	private void Disable()
	{
		try
		{
			_cancellationTokenSource?.Cancel();
		}
		catch { }
	}

	private async Task<IDictionary<string, string>> RetrieveData(bool isBlocking = false)
	{
		IDictionary<string, string> values = new Dictionary<string, string>();

		try
		{
			QueryOptions queryOptions = new()
			{
				WaitIndex = isBlocking ? _consulConfigurationIndex : 0ul,
				WaitTime = _consulClientConfiguration.Wait
			};

			var env = _consulClientConfiguration.Enviroment + "/";

			var result = await _consulClient
					  .KV
					  .List(env, queryOptions, _cancellationTokenSource!.Token);

			if (result.StatusCode == HttpStatusCode.OK && result.LastIndex > _consulConfigurationIndex)
			{
				foreach (var temp in result.Response)
				{
					string value = string.Empty;
					if (temp.Value != null && temp.Value.Length > 0)
					{
						value = Encoding.UTF8.GetString(temp.Value);
					}

					string key = temp.Key[env.Length..].Replace("/", ":");


					values.Add(key, value);
				}

				_consulConfigurationIndex = result.LastIndex;
			}
		}
		catch (Exception e)
		{
			NLog.LogManager.GetLogger("*").Error(e);
		}

		return values;
	}

	internal static ConsulConfigurationProvider Create(ConsulClientConfiguration consulConfigurationClient)
	{
		consulConfigurationProvider = new ConsulConfigurationProvider(consulConfigurationClient);

		return consulConfigurationProvider;
	}

	public static void Register(IHostApplicationLifetime lifetime)
	{
		try
		{
			consulConfigurationProvider?.RegisterOnStopped(lifetime);
		}
		catch { }
	}

	public void Dispose()
	{
		Dispose(true);
	}

	private void Dispose(bool disposing)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}

		if (!disposing)
		{
			GC.SuppressFinalize(this);
		}
	}
}
