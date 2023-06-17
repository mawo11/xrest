namespace XRest.Consul;

internal interface IServiceClientFactory
{
	T Create<T>(string serviceName) where T : class;
}