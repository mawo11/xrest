
using Dapper;
using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Identity.Infratructure.MsSql;

internal sealed class AccountOperationRepository : IAccountOperationRepository
{
	private readonly ISqlConnectionFactory _connectionFactory;
	private readonly ILogger<AccountOperationRepository> _logger;

	public AccountOperationRepository(ISqlConnectionFactory connectionFactory, ILogger<AccountOperationRepository> logger)
	{
		_connectionFactory = connectionFactory;
		_logger = logger;
	}

	public async ValueTask<bool> AddAccountOperationAsync(AccountOperation accountOperation)
	{
		try
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();

			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@accountId", accountOperation.AccountId, DbType.Int32);
			dynamicParameters.Add("@token ", accountOperation.Token, DbType.String);
			dynamicParameters.Add("@created", accountOperation.Created, DbType.DateTime);
			dynamicParameters.Add("@operationType", accountOperation.OperationType, DbType.Byte);

			var result = await connection.ExecuteAsync(
					  "ident.AddAccountOperation",
					  dynamicParameters,
					  commandType: CommandType.StoredProcedure);

			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "AddAccountOperation");
		}

		return false;
	}

	public async ValueTask<AccountOperation> GetAccountOperationByTokenAsync(string token)
	{
		try
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@token ", token, DbType.String);

			return await connection.QueryFirstOrDefaultAsync<AccountOperation>(
					  "ident.GetAccountOperationByToken",
					  dynamicParameters,
					  commandType: CommandType.StoredProcedure);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetAccountOperationByToken");
		}

		return AccountOperation.Invalid;
	}

	public async ValueTask<bool> RemoveAccountOperationByTokenAsync(string token)
	{
		try
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@token ", token, DbType.String);

			await connection.ExecuteAsync(
					  "ident.RemoveAccountOperationByToken",
					  dynamicParameters,
					  commandType: CommandType.StoredProcedure);

			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "RemoveAccountOperationByToken");
		}

		return false;
	}
}
