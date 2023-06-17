using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System.Reflection;

namespace XRest.Shared.Extensions;

public static class LoggingConfigurationExtensions
{
	public static IConfigurationBuilder ConfigureNlog(this ConfigurationManager configuration)
	{
		NLog.LogManager.Configuration.Variables["logDay"] = configuration.GetValue<string>("apps/logConfig/logFolder");
		NLog.LogManager.Configuration.Variables["logDbConnectionString"] = configuration.GetValue<string>("apps/logConfig/logDbConnectionString");
		string logLevel = configuration.GetValue<string>("apps/logConfig/logDbLevel", "off");
		NLog.LogManager.Configuration.Variables["logDbLevel"] = logLevel;
		NLog.LogManager.Configuration.Variables["appName"] = Assembly.GetEntryAssembly()!.GetName().Name;

		if (logLevel.ToLower() != "off")
		{
			NLog.LogManager.Configuration.AddRuleForAllLevels(new NLog.Targets.DatabaseTarget());
		}

		return configuration;
	}

	public static ILoggingBuilder AddLogging(this ILoggingBuilder logging)
	{
		if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?.ToLower() == "true")
		{
			logging.AddConsole();
		}
		else
		{
			LogManager
				.Setup()
				.LoadConfigurationFromAppSettings();

			logging.AddNLogWeb();
		}


		string? seq = Environment.GetEnvironmentVariable("SEQ_URL");
		string? seqApiKey = Environment.GetEnvironmentVariable("SEQ_API_KEY");
		if (!string.IsNullOrEmpty(seq) && !string.IsNullOrEmpty(seqApiKey))
		{
			logging.AddSeq(seq, seqApiKey);
		}

		return logging;
	}
}
