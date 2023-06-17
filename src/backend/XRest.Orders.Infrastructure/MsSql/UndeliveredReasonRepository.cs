using System.Data;
using Dapper;
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.Infrastructure.MsSql;

internal sealed class UndeliveredReasonRepository : IUndeliveredReasonRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<FavoriteAddressRepository> _logger;

	public UndeliveredReasonRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<FavoriteAddressRepository> logger)
	{
		_sqlConnectionFactory = sqlConnectionFactory;
		_logger = logger;
	}

	public async ValueTask<UndeliveredReason[]> GetUndeliveredReasonsAsync(SourceType? sourceType = null)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@sourceType", sourceType, DbType.Byte);

			var result = await connection.QueryAsync<UndeliveredReason>(
				 "ord.GetUndeliveredReasonsBySource",
				 dynamicParameters,
				 commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);

			return result.ToArray();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetUndeliveredReasonsAsync");
		}

		return Array.Empty<UndeliveredReason>();
	}

	public async ValueTask<string?> GetReasonById(int id)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@id", id, DbType.Int32);

			var result = await connection.QueryFirstOrDefaultAsync<string>(
				 "ord.GetUndeliveredReasonById",
				 dynamicParameters,
				 commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);

			return result;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetUndeliveredReasonsAsync");
		}

		return null;
	}
}