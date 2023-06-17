using Dapper;
using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Identity.Infratructure.MsSql;

internal sealed class AccountRepository : IAccountRepository
{
	private readonly ISqlConnectionFactory _connectionFactory;
	private readonly ILogger<AccountRepository> _logger;

	public AccountRepository(ISqlConnectionFactory connectionFactory, ILogger<AccountRepository> logger)
	{
		_logger = logger;
		_connectionFactory = connectionFactory;
	}

	public async ValueTask AddPointsAsync(int userId, int points, string comment, DateTime creation, int? orderId = null)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", userId, DbType.Int32);
		dynamicParameters.Add("@points ", points, DbType.Int32);
		dynamicParameters.Add("@comment", comment, DbType.String);
		dynamicParameters.Add("@creation", creation, DbType.DateTime);
		dynamicParameters.Add("@orderId", orderId, DbType.Int32);

		var result = await connection.ExecuteAsync(
				  "ident.AddPoints",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask<Account?> GetAccountByEmailAsync(string email)
	{
		try
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@email", email, DbType.String);

			return await connection.QueryFirstAsync<Account>(
					  "ident.GetAccountByEmail",
					  dynamicParameters,
					  commandType: CommandType.StoredProcedure);
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "GetAccountByEmail");
		}

		return null;
	}

	public async ValueTask<Account?> GetAccountByIdAsync(int accountId)
	{
		try
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@accountId", accountId, DbType.Int32);

			return await connection.QueryFirstAsync<Account>(
					  "ident.GetAccountById",
					  dynamicParameters,
					  commandType: CommandType.StoredProcedure);
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "GetAccountByEmail");
		}

		return null;
	}

	public async ValueTask<Account?> GetAccountByExternalIdAsync(ExternalProvider exernalProvider, string externalId)
	{
		try
		{
			using var connection = await _connectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@externalId", externalId, DbType.String);
			dynamicParameters.Add("@provider", exernalProvider, DbType.Byte);

			return await connection.QueryFirstOrDefaultAsync<Account>(
					  "ident.GetAccountByExternalIdentifier",
					  dynamicParameters,
					  commandType: CommandType.StoredProcedure) ?? Account.Invalid;
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "GetAccountByEmail");
		}

		return Account.Invalid;
	}

	public async ValueTask<int> InsertNewAccountAsync(NewAccountData newAccountData)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@firstname", newAccountData.Firstname, DbType.String);
		dynamicParameters.Add("@lastname", newAccountData.Lastname, DbType.String);
		dynamicParameters.Add("@email", newAccountData.Email, DbType.String);
		dynamicParameters.Add("@password", newAccountData.Password, DbType.String);
		dynamicParameters.Add("@marketing", newAccountData.Marketing, DbType.Boolean);
		dynamicParameters.Add("@terms", newAccountData.Terms, DbType.Boolean);
		dynamicParameters.Add("@externalId", newAccountData.ExternalId, DbType.String);
		dynamicParameters.Add("@provider", newAccountData.ExernalProvider, DbType.Byte);

		var result = await connection.QueryFirstOrDefaultAsync<int>(
				  "ident.CustomerNewAccount",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);

		return result;
	}

	public async ValueTask<bool> AddNewLoginMethodToAccountAsync(int accountId, string externalId, ExternalProvider externalProvider)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);
		dynamicParameters.Add("@externalId", externalId, DbType.String);
		dynamicParameters.Add("@provider", externalProvider, DbType.Byte);

		var result = await connection.QueryFirstOrDefaultAsync<int>(
				  "ident.CustomerAddNewLoginMethodToAccount",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);

		return true;
	}

	public async ValueTask SetAccountStatusAsync(int accountId, AccountStatus status)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);
		dynamicParameters.Add("@status ", status, DbType.Byte);

		await connection.ExecuteAsync(
				  "ident.SetAccountStatus",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask UpdateAccountPasswordAsync(int accountId, string password)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);
		dynamicParameters.Add("@password ", password, DbType.String);

		await connection.ExecuteAsync(
				  "ident.UpdateAccountPassword",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask DisableAccountAsync(int accountId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);

		var result = await connection.ExecuteAsync(
				  "ident.DisableAccount",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask SaveAccountAsync(Account account)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@id", account.Id, DbType.Int32);
		dynamicParameters.Add("@email", account.Email, DbType.String);
		dynamicParameters.Add("@password", account.Password, DbType.String);
		dynamicParameters.Add("@created_date", account.Created, DbType.DateTime);
		dynamicParameters.Add("@locked", account.Islocked, DbType.Boolean);
		dynamicParameters.Add("@deleted", account.IsDeleted, DbType.Boolean);
		dynamicParameters.Add("@locked_reason", account.LockedReason, DbType.String);
		dynamicParameters.Add("@phone", account.Phone, DbType.String);
		dynamicParameters.Add("@display_name", account.DisplayName, DbType.String);
		dynamicParameters.Add("@points", account.Points, DbType.Int32);
		dynamicParameters.Add("@must_change_password", account.MustChangePassword, DbType.Boolean);
		dynamicParameters.Add("@firstname", account.Firstname, DbType.String);
		dynamicParameters.Add("@lastname", account.Lastname, DbType.String);
		dynamicParameters.Add("@status", account.Status, DbType.Byte);
		dynamicParameters.Add("@terms_accepted", account.TermsAccepted, DbType.Boolean);
		dynamicParameters.Add("@marketing", account.Marketing, DbType.Boolean);
		dynamicParameters.Add("@birth_date", account.BirthDate, DbType.DateTime2);

		await connection.ExecuteAsync(
				  "ident.SaveAccount",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}


	public async ValueTask<IEnumerable<CustomerPoint>> GetAccountPointsHistoryAsync(int accountId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);

		return await connection.QueryAsync<CustomerPoint>(
				   "ident.CustomerGetPointsHistory",
				   dynamicParameters,
				   commandType: CommandType.StoredProcedure);
	}

	public async ValueTask IncreaseCustomerMarketingAgreementTriesAsync(int accountId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);

		await connection.ExecuteAsync(
				  "ident.IncreaseCustomerMarketingAgreementTries",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask<IEnumerable<MarketingAgreement>> GetMarketingAgreementAsync(int accountId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);

		return await connection.QueryAsync<MarketingAgreement>(
				  "ident.GetCustomerMarketingAgreement",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask AddCustomerMarketingAgreementAsync(int accountId, int marketingId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);
		dynamicParameters.Add("@marketingId", marketingId, DbType.Int32);

		await connection.ExecuteAsync(
				  "ident.AddCustomerMarketingAgreement",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask RemoveAllCustomerMarketingAgreementAsync(int accountId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);

		await connection.ExecuteAsync(
				  "ident.RemoveAllCustomerMarketingAgreement",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}

	public async ValueTask DisableAgreementCounterAsync(int accountId)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@accountId", accountId, DbType.Int32);

		await connection.ExecuteAsync(
				  "ident.DisableAgreementCounter",
				  dynamicParameters,
				  commandType: CommandType.StoredProcedure);
	}
}
