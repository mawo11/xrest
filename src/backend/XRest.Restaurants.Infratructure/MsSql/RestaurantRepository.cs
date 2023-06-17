using Dapper;
using XRest.Restaurants.App.Domain;
using XRest.Restaurants.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Restaurants.Infratructure.MsSql;

internal sealed class RestaurantRepository : IRestaurantRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<RestaurantRepository> _logger;

	public RestaurantRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<RestaurantRepository> logger)
	{
		_logger = logger;
		_sqlConnectionFactory = sqlConnectionFactory;
	}

	public async ValueTask<RestaurantOrderDayInformation?> GetRestaurantOrderInformationByIdAsync(int restaurantId)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("@restaurantId", restaurantId, DbType.Int32);

			return await connection.QueryFirstAsync<RestaurantOrderDayInformation>(
				"rest.GetRestaurantOrderInformationById",
				dynamicParameters,
				commandType: CommandType.StoredProcedure
				).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			_logger.LogCritical("{message}", ex.Message);
			return null;
		}
	}

	public async ValueTask<Restaurant[]> LoadAllRestaurantsAsync()
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			var resultSet = await connection.QueryMultipleAsync("rest.LoadAllRestuarant",
				commandType: CommandType.StoredProcedure).ConfigureAwait(false);

			IEnumerable<Restaurant> restaurants = await resultSet.ReadAsync<Restaurant>();
			IEnumerable<RestaurantExcludeTime> restaurantExcludeTimes = await resultSet.ReadAsync<RestaurantExcludeTime>();
			IEnumerable<RestaurantWorkingTime> restaurantWorkingTimes = await resultSet.ReadAsync<RestaurantWorkingTime>();


			foreach (var restaurant in restaurants)
			{
				restaurant.Excludes = restaurantExcludeTimes
						.Where(x => x.RestaurantId == restaurant.Id)
						.ToList();

				restaurant.Workings = restaurantWorkingTimes
					   .Where(x => x.RestaurantId == restaurant.Id)
					   .ToList();
			}

			return restaurants.ToArray();
		}
		catch (Exception e)
		{
			_logger.LogCritical("{message}", e.Message);
			return Array.Empty<Restaurant>();
		}
	}
}
