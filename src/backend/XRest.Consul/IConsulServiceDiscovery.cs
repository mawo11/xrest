namespace XRest.Consul;

public interface IConsulServiceDiscovery
{
	ValueTask<string?> GetAddressAsync(string serviceName);
}