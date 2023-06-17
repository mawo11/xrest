namespace XRest.Consul;

public class ConsulClientConfiguration
{
	public string? Url { get; set; }

	public string? Token { get; set; }

	public string? Enviroment { get; set; }

	public bool ReloadOnChange { get; set; }

	public TimeSpan? Wait { get; set; }

	public string? AppIp { get; set; }

	public int AppPort { get; set; }
}
