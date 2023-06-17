using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XRest.Shared.Extensions;

public static class CustomJsonSerializer
{
	private static readonly JsonSerializerOptions JsonSerializerOptions;

	static CustomJsonSerializer()
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

	public static string Serialize<TValue>(TValue data)
	{
		return JsonSerializer.Serialize(data, JsonSerializerOptions);
	}

	public static TValue? Deserialize<TValue>(string data)
	{
		return JsonSerializer.Deserialize<TValue>(data, JsonSerializerOptions);
	}
}
