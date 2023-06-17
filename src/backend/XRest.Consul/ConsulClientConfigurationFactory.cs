namespace XRest.Consul;

public class ConsulClientConfigurationFactory
{
	private static ConsulClientConfiguration? _consulClientConfiguration;
	private static object _lock = new object();

	public static ConsulClientConfiguration Get()
	{
		lock (_lock)
		{
			if (_consulClientConfiguration == null)
			{

				string? consulServiceName = Environment.GetEnvironmentVariable("CONSUL_ADDR", EnvironmentVariableTarget.Process);
				if (!string.IsNullOrEmpty(consulServiceName))
				{

					_consulClientConfiguration = ReadFromEnv();
				}

				if (_consulClientConfiguration == null)
				{
					throw new InvalidProgramException("Brak konfiguracji consul (env/json)");
				}


			}
		}

		return _consulClientConfiguration;
	}

	private static ConsulClientConfiguration ReadFromEnv()
	{
		bool GetEnvironmentVariableAsBool(string name, bool defaultValue)
		{
			var temp = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
			if (string.IsNullOrEmpty(temp))
			{
				return defaultValue;
			}

			return temp.ToLower() == "true";
		}

		int GetEnvironmentVariableAsInt(string name, int defaultValue)
		{
			var temp = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
			if (string.IsNullOrEmpty(temp))
			{
				return defaultValue;
			}

			if (int.TryParse(temp, out int newValue))
			{
				return newValue;
			}

			return defaultValue;
		}

		string? consulServiceAddr = Environment.GetEnvironmentVariable("CONSUL_ADDR", EnvironmentVariableTarget.Process);

		return _consulClientConfiguration = new ConsulClientConfiguration
		{
			Url = consulServiceAddr,
			Enviroment = Environment.GetEnvironmentVariable("CONSUL_ENV", EnvironmentVariableTarget.Process),
			ReloadOnChange = GetEnvironmentVariableAsBool("CONSUL_RELOADONCHANGE", true),
			Token = Environment.GetEnvironmentVariable("CONSUL_TOKEN", EnvironmentVariableTarget.Process),
			Wait = TimeSpan.FromSeconds(GetEnvironmentVariableAsInt("CONSUL_WAITSECONDS", 30)),
			AppIp = Environment.GetEnvironmentVariable("APP_ADDR", EnvironmentVariableTarget.Process),
			AppPort = GetEnvironmentVariableAsInt("APP_PORT", 0),
		};
	}
}
