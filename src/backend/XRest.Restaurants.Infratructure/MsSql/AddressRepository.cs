using XRest.Restaurants.App.Domain;
using XRest.Restaurants.App.Services;
using Dapper;
using System.Data;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.Infratructure.MsSql;

internal sealed class AddressRepository : IAddressRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<AddressRepository> _logger;

	public AddressRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<AddressRepository> logger)
	{
		_logger = logger;
		_sqlConnectionFactory = sqlConnectionFactory;
	}

	public async ValueTask<bool> IsNewDataAvailableAsync(DateTime auditDate)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("@audit", auditDate, DbType.DateTime2);

			return await connection.QueryFirstAsync<bool>(
				"loc.CheckForChangesInAddresses",
				dynamicParameters,
				commandType: CommandType.StoredProcedure
				);
		}
		catch (Exception ex)
		{
			_logger.LogCritical("{message}", ex.Message);
			return false;
		}
	}

	public async ValueTask<IEnumerable<Address>> LoadAllAdressesAsync()
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			return await connection.QueryAsync<Address>(
				"loc.LoadAllAddresses",
				commandType: CommandType.StoredProcedure
				);
		}
		catch (Exception ex)
		{
			_logger.LogCritical("{message}", ex.Message);
			return Array.Empty<Address>();
		}
	}
}
