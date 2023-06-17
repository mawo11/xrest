using XRest.Orders.App.Domain;
using XRest.Restaurants.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace XRest.Orders.App.Services;

internal class PaymentService : IPaymentService
{
	private static readonly HttpClient _httpClient;

	static PaymentService()
	{
		_httpClient = new HttpClient();
		_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");
		_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
		_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));

	}

	private readonly ILogger<PaymentService> _logger;
	private readonly IConfiguration _configuration;
	private readonly IPaymentInformationService _paymentInformationService;

	public PaymentService(ILogger<PaymentService> logger, IConfiguration configuration, IPaymentInformationService paymentInformationService)
	{
		_logger = logger;
		_configuration = configuration;
		_paymentInformationService = paymentInformationService;
	}

	public async ValueTask<RegisterPaymentResult> RegisterPaymentAsync(int orderId, NewOnlineOrder newOnlineOrder)
	{
		string paymentUrl = _configuration.GetValue("Przelewy24:Przelewy24Url", string.Empty);
		string returnUrl = _configuration.GetValue("Przelewy24:ReturnUrl", string.Empty);
		string statusUrl = _configuration.GetValue("Przelewy24:StatusUrl", string.Empty);
		var paymentInfo = await _paymentInformationService.GetPaymentInformation(newOnlineOrder.RestaurantId);

		if (string.IsNullOrEmpty(paymentUrl) || string.IsNullOrEmpty(statusUrl) ||
			string.IsNullOrEmpty(statusUrl) || !paymentInfo.Exists)
		{
			_logger.LogCritical($"Brak ustawionej wartości w {paymentUrl},{returnUrl},{statusUrl},{!paymentInfo.Exists}");
			return new()
			{
				Success = false,
				Response = "Błędna konfiguracja płatności"
			};
		}

		if (!paymentUrl.EndsWith("/"))
		{
			paymentUrl += "/";
		}

		List<KeyValuePair<string, string>> values = new();
		int orderAmount = (int)(newOnlineOrder.Amount * 100);

		values.Add(new KeyValuePair<string, string>("p24_merchant_id", paymentInfo.PaymentId!));
		values.Add(new KeyValuePair<string, string>("p24_pos_id", paymentInfo.PaymentId!));
		values.Add(new KeyValuePair<string, string>("p24_session_id", orderId.ToString()));
		values.Add(new KeyValuePair<string, string>("p24_amount", orderAmount.ToString()));
		values.Add(new KeyValuePair<string, string>("p24_currency", "PLN"));
		values.Add(new KeyValuePair<string, string>("p24_description", "Usługa gastronomiczna"));
		values.Add(new KeyValuePair<string, string>("p24_email", newOnlineOrder.Email!));
		values.Add(new KeyValuePair<string, string>("p24_country", "PL"));
		values.Add(new KeyValuePair<string, string>("p24_url_return", string.Format(returnUrl, orderId)));
		values.Add(new KeyValuePair<string, string>("p24_url_status", statusUrl));
		values.Add(new KeyValuePair<string, string>("p24_api_version", "3.2"));
		values.Add(new KeyValuePair<string, string>("p24_encoding", "UTF-8"));
		string sign = Md5($"{orderId}|{paymentInfo.PaymentId}|{orderAmount}|PLN|{paymentInfo.PaymentSecret}");

		_logger.LogInformation("rejestracja platnosci: {orderId}|{PaymentId}|{orderAmount}", orderId, paymentInfo.PaymentId, orderAmount);
		values.Add(new KeyValuePair<string, string>("p24_sign", sign));

		var content = new FormUrlEncodedContent(values);
		if (content.Headers.ContentType != null)
		{
			content.Headers.ContentType.CharSet = "UTF-8";
		}

		var result = await _httpClient.PostAsync(paymentUrl + "trnRegister", content);
		result.EnsureSuccessStatusCode();

		string response = await result.Content.ReadAsStringAsync();
		_logger.LogInformation("PaymentResponse: {response}", response);

		if (!string.IsNullOrEmpty(response))
		{
			bool error = false;
			string? token = null;
			string[] parts = response.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string part in parts)
			{
				string[] tmps = part.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
				if (tmps[0] == "error")
				{
					error = tmps[1] != "0";
					continue;
				}

				if (tmps[0] == "token")
				{
					token = tmps[1];
				}
			}

			if (!error)
			{
				return new()
				{
					Success = true,
					Token = token
				};
			}
			else
			{
				return new()
				{
					Success = false,
					Response = response
				};
			}
		}

		return new()
		{
			Success = false,
			Response = "Brak odpowiedzi z systemu płatności"
		};
	}

	public async ValueTask ConfirmPaymentAsync(int orderId, int paymentyOrderId, decimal amount, int restaurantId)
	{
		string paymentUrl = _configuration.GetValue("Przelewy24:Przelewy24Url", string.Empty);
		var paymentInfo = await _paymentInformationService.GetPaymentInformation(restaurantId);

		if (string.IsNullOrEmpty(paymentUrl) || !paymentInfo.Exists)
		{
			_logger.LogCritical("Brak ustawionej wartości wpaymentUrl ");
			return;
		}

		if (!paymentUrl.EndsWith("/"))
		{
			paymentUrl += "/";
		}


		Dictionary<string, string> values = new();
		int orderAmount = (int)(amount * 100);

		values.Add("p24_merchant_id", paymentInfo.PaymentId!);
		values.Add("p24_pos_id", paymentInfo.PaymentId!);
		values.Add("p24_session_id", orderId.ToString());
		values.Add("p24_amount", orderAmount.ToString());
		values.Add("p24_currency", "PLN");
		values.Add("p24_order_id", paymentyOrderId.ToString());

		string sign = Md5($"{orderId}|{paymentyOrderId}|{orderAmount}|PLN|{paymentInfo.PaymentSecret}");
		values.Add("p24_sign", sign);

		var result = await _httpClient.PostAsync(paymentUrl + "trnVerify", new FormUrlEncodedContent(values));
		result.EnsureSuccessStatusCode();

		string response = await result.Content.ReadAsStringAsync();

		_logger.LogInformation("ConfirmPayment: {response}", response);
	}

	internal static string Md5(string value)
	{
		using MD5 mD5 = MD5.Create();
		byte[] numArray = mD5.ComputeHash(Encoding.Default.GetBytes(value));
		StringBuilder stringBuilder = new();
		for (int i = 0; i < numArray.Length; i++)
		{
			stringBuilder.Append(numArray[i].ToString("x2"));
		}

		return stringBuilder.ToString();
	}
}
