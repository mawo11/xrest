using Dapper;
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Orders.Infrastructure.MsSql;

internal sealed class FavoriteAddressRepository : IFavoriteAddressRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<FavoriteAddressRepository> _logger;

	public FavoriteAddressRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<FavoriteAddressRepository> logger)
	{
		_sqlConnectionFactory = sqlConnectionFactory;
		_logger = logger;
	}

	public async ValueTask<IEnumerable<FavoriteAddressItem>> GetForAccountAsync(int accountId)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@accountId", accountId, DbType.Int32);

			return await connection.QueryAsync<FavoriteAddressItem>(
				"ord.GetFavAddressForAccount",
				dynamicParameters,
				commandType: CommandType.StoredProcedure);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetForAccountAsync");
		}

		return Array.Empty<FavoriteAddressItem>();
	}

	public async ValueTask RemoveAsync(int accountId, int id)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();

			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@accountId", accountId, DbType.String);
			dynamicParameters.Add("@id", id, DbType.String);

			await connection.ExecuteAsync(
				"ord.RemoveFavAddressForAccount",
				dynamicParameters,
				commandType: CommandType.StoredProcedure);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "RemoveAsync");
		}
	}

	public async ValueTask SaveAsync(FavoriteAddressItem favoriteAddressItem)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@account_id", favoriteAddressItem.AccountId, DbType.Int32);
			dynamicParameters.Add("@id", favoriteAddressItem.Id, DbType.Int32);
			dynamicParameters.Add("@address_city", favoriteAddressItem.AddressCity, DbType.String);
			dynamicParameters.Add("@address_street", favoriteAddressItem.AddressStreet, DbType.String);
			dynamicParameters.Add("@address_street_number", favoriteAddressItem.AddressStreetNumber, DbType.String);
			dynamicParameters.Add("@address_house_number", favoriteAddressItem.AddressHouseNumber, DbType.String);
			dynamicParameters.Add("@default", favoriteAddressItem.Default, DbType.Boolean);

			await connection.ExecuteAsync(
				"ord.SaveFavAddressForAccount",
				dynamicParameters,
				commandType: CommandType.StoredProcedure);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "SaveAsync");
		}
	}
}
