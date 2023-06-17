using Dapper;
using XRest.Restaurants.App.Domain;
using XRest.Restaurants.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Restaurants.Infratructure.MsSql;

internal sealed class PaymentRepository : IPaymentRepository
{
	private readonly ISqlConnectionFactory _connectionFactory;
	private readonly IConfiguration _configuration;
	private readonly IMemoryCache _memoryCache;
	private readonly ILogger<PaymentRepository> _logger;

	public PaymentRepository(ISqlConnectionFactory connectionFactory,
		IConfiguration configuration,
		IMemoryCache memoryCache,
		ILogger<PaymentRepository> logger)
	{
		_logger = logger;
		_connectionFactory = connectionFactory;
		_configuration = configuration;
		_memoryCache = memoryCache;
	}

	public async ValueTask<Payment[]> GetAllPaymentsAsync()
	{
		return await _memoryCache.GetOrCreateAsync(MemoryCacheKeys.Payment, async (cache) =>
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			var result = (await connection.QueryAsync<Payment>(
				  "rest.GetAllPayments",
				  commandType: CommandType.StoredProcedure)).ToArray();

			cache.Value = result;
			cache.SetAbsoluteExpiration(TimeSpan.FromMinutes(_configuration.GetValue<int>("Restaurants:PaymentCacheMinutes")));

			return result;
		});
	}
}
