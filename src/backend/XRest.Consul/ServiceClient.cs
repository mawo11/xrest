using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XRest.Consul;

public abstract class ServiceClient
{

	private static readonly JsonSerializerOptions JsonSerializerOptions;

	static ServiceClient()
	{
		JsonSerializerOptions = new()
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			AllowTrailingCommas = true,
			MaxDepth = 64,
			IncludeFields = true,
			NumberHandling = JsonNumberHandling.AllowReadingFromString,
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};
		JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
	}

	private readonly string _serviceName;
	private readonly IConsulServiceDiscovery _serviceDiscovery;
	private readonly HttpClient _httpClient;
	private readonly ILogger<ServiceClient> _logger;

	protected ServiceClient(string serviceName,
		IConsulServiceDiscovery consulServiceDiscovery,
		HttpClient httpClient,
		ILogger<ServiceClient> logger)
	{
		_serviceName = serviceName;
		_serviceDiscovery = consulServiceDiscovery;
		_httpClient = httpClient;
		_logger = logger;
	}

	protected async ValueTask<ServiceCallResult> SendAsync(string url, HttpMethod httpMethod, object? body, string? token, CancellationToken cancellationToken)
	{
		HttpRequestMessage httpRequestMessage = await PrepareRequest(url, httpMethod, body, token);
		using var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
		_logger.LogDebug("Response status code: {response.StatusCode}", response.StatusCode);

		return Map(response);
	}

	protected async ValueTask<ServiceCallResultData<T?>> SendAsync<T>(string url, HttpMethod httpMethod, object? body, string? token, CancellationToken cancellationToken)
	{
		HttpRequestMessage httpRequestMessage = await PrepareRequest(url, httpMethod, body, token);
		using var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
		_logger.LogDebug("Response status code: {response.StatusCode}", response.StatusCode);
		if (response.IsSuccessStatusCode)
		{
			string serializedContent = await response.Content.ReadAsStringAsync(cancellationToken);
			_logger.LogDebug("Response: {serializedContent}", serializedContent);
			T? content = JsonSerializer.Deserialize<T>(serializedContent, JsonSerializerOptions);
			return Map<T>(response, content);
		}

		return Map<T>(response, default);
	}

	private async ValueTask<HttpRequestMessage> PrepareRequest(string url, HttpMethod httpMethod, object? body, string? token)
	{
		var serviceUrl = await _serviceDiscovery.GetAddressAsync(_serviceName);
		if (string.IsNullOrEmpty(serviceUrl))
		{
			throw new InvalidOperationException("serviceUrl is empty");
		}

		var requestUrl = $"{serviceUrl.TrimEnd('/')}{url}";
		_logger.LogDebug("Request url: {requestUrl}", requestUrl);

		HttpRequestMessage httpRequestMessage = new()
		{
			Method = httpMethod,
			RequestUri = new Uri(requestUrl)
		};

		if (!string.IsNullOrEmpty(token))
		{
			httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		if (body != null)
		{
			var content = JsonSerializer.Serialize(body, JsonSerializerOptions);
			_logger.LogDebug("Request content: {content}", content);

			httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
		}

		return httpRequestMessage;
	}

	private static ServiceCallResult Map(HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return ServiceCallResult.Ok;
		}

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			return ServiceCallResult.Unauthorized;
		}

		return ServiceCallResult.Error;
	}

	private static ServiceCallResultData<T?> Map<T>(HttpResponseMessage response, T? content)
	{
		if (response.IsSuccessStatusCode)
		{
			return new ServiceCallResultData<T?>
			{
				Status = ServiceCallStatus.Ok,
				Data = content
			};
		}

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			return ServiceCallResultData<T?>.RdUnauthorized;
		}

		return ServiceCallResultData<T?>.RdError;
	}
}
