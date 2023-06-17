using Dapper;
using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Identity.Infratructure.MsSql;

internal sealed class TokenRepository : ITokenRepository
{
	private readonly ISqlConnectionFactory _connectionFactory;
	private readonly ILogger<TokenRepository> _logger;

	public TokenRepository(ISqlConnectionFactory connectionFactory, ILogger<TokenRepository> logger)
	{
		_logger = logger;
		_connectionFactory = connectionFactory;
	}

	public async ValueTask AddNewAsync(RefreshTokenData refreshTokenData)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@token", refreshTokenData.Token, DbType.String);
		dynamicParameters.Add("@created ", refreshTokenData.Created, DbType.DateTime);
		dynamicParameters.Add("@expired", refreshTokenData.Expired, DbType.DateTime);

		await connection.ExecuteAsync(
				 "ident.TokenAddNew",
				 dynamicParameters,
				 commandType: CommandType.StoredProcedure);
	}

	public async ValueTask<RefreshTokenData> GetByTokenAsync(string token)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@token", token, DbType.String);

		return await connection.QueryFirstOrDefaultAsync<RefreshTokenData>(
				  "ident.TokenGetByToken",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask RemoveByIdAsync(int id)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@id", id, DbType.Int32);

		await connection.ExecuteAsync(
				  "ident.TokenRemoveById",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}
}
