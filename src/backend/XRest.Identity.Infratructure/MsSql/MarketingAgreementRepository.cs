using Dapper;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Responses;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace XRest.Identity.Infratructure.MsSql;

internal sealed class MarketingAgreementRepository : IMarketingAgreementRepository
{
	private readonly ISqlConnectionFactory _connectionFactory;
	private readonly IConfiguration _configuration;
	private readonly IMemoryCache _memoryCache;

	public MarketingAgreementRepository(ISqlConnectionFactory connectionFactory, IConfiguration configuration, IMemoryCache memoryCache)
	{
		_connectionFactory = connectionFactory;
		_configuration = configuration;
		_memoryCache = memoryCache;
	}

	public async ValueTask<MarketingAgreement[]> GetMarketingAgreementsAsync()
	{
		return await _memoryCache.GetOrCreateAsync(MemoryCacheKeys.Terms, async (cache) =>
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			var result = (await connection.QueryAsync<MarketingAgreement>(
				  "ident.GetTerms",
				  commandType: CommandType.StoredProcedure)).ToArray();

			cache.Value = result;
			cache.SetAbsoluteExpiration(TimeSpan.FromMinutes(_configuration.GetValue<int>("Identity:TermsCacheMinutes")));

			return result;
		});
	}
}
