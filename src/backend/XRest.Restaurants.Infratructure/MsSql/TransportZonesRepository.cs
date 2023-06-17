using Dapper;
using XRest.Restaurants.App.Domain;
using XRest.Restaurants.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Restaurants.Infratructure.MsSql;

internal sealed class TransportZonesRepository : ITransportZonesRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<TransportZonesRepository> _logger;

	public TransportZonesRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<TransportZonesRepository> logger)
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
				"rest.CheckForChangesInTransportZones",
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

	public async ValueTask<IEnumerable<RestaurantTransport>> LoadAllAsync()
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			var resultSet = await connection.QueryMultipleAsync("rest.LoadAllTransportZones",
				commandType: CommandType.StoredProcedure);

			IEnumerable<RestaurantTransport> restaurantTransports = await resultSet.ReadAsync<RestaurantTransport>();
			IEnumerable<RestaurantTransportPrice> restaurantTransportPrices = await resultSet.ReadAsync<RestaurantTransportPrice>();
			IEnumerable<RestaurantTransportZone> restaurantTransportZones = await resultSet.ReadAsync<RestaurantTransportZone>();

			foreach (var restaurantTransport in restaurantTransports)
			{
				restaurantTransport.Prices = restaurantTransportPrices
						.Where(x => x.RestaurantTransportId == restaurantTransport.Id)
						.ToArray();

				restaurantTransport.Zones = restaurantTransportZones
					   .Where(x => x.RestaurantTransportId == restaurantTransport.Id)
					   .ToArray();
			}



			return restaurantTransports;
		}
		catch (Exception ex)
		{
			_logger.LogCritical("{message}", ex.Message);
			return Array.Empty<RestaurantTransport>();
		}
	}
}
