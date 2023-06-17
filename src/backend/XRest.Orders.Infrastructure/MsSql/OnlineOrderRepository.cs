using Dapper;
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Orders.Infrastructure.MsSql.Internal.Online;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Orders.Infrastructure.MsSql;

internal sealed class OnlineOrderRepository : IOnlineOrderRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly IOnlineOrderSerializator _onlineOrderSerializator;
	private readonly ILogger<OnlineOrderRepository> _logger;

	public OnlineOrderRepository(ISqlConnectionFactory sqlConnectionFactory,
		ILogger<OnlineOrderRepository> logger)
	{
		_logger = logger;
		_sqlConnectionFactory = sqlConnectionFactory;
		_onlineOrderSerializator = new OnlineOrderSerializator();
	}

	public async ValueTask<int> SaveAsync(NewOnlineOrder newOnlineOrder)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			var (header, rows) = _onlineOrderSerializator.Serialize(newOnlineOrder);

			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@header", header.AsTableValuedParameter("ord.NewOnlineOrderHeader"));
			dynamicParameters.Add("@rows", rows.AsTableValuedParameter("ord.NewOnlineOrderRow"));
			dynamicParameters.Add("@marketingIds", Serialize(newOnlineOrder.MarketingIds).AsTableValuedParameter("dbo.inttable"));

			return await connection.QueryFirstAsync<int>("ord.CreateOnlineOrder", dynamicParameters, commandType: CommandType.StoredProcedure);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "SaveAsync");
		}

		return -1;
	}

	private static DataTable Serialize(int[] ids)
	{
		DataTable table = new();
		table.Columns.Add("id", typeof(int));

		if (ids == null)
		{
			return table;
		}

		foreach (int id in ids)
		{
			table.Rows.Add(id);
		}

		return table;
	}

	public async ValueTask<bool> UpdatePaymentStatusAsync(int orderId, OrderStatus status, bool payed, string paymentInfo, DateTime modifyDate)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@orderId", orderId, DbType.Int32);
			dynamicParameters.Add("@orderStatus", status, DbType.Byte);
			dynamicParameters.Add("@payed", payed, DbType.Byte);
			dynamicParameters.Add("@paymentPayload", paymentInfo, DbType.String);
			dynamicParameters.Add("@modifyDate", modifyDate, DbType.DateTime);

			await connection.ExecuteAsync("ord.OnlineOrderUpdatePaymentStatus", dynamicParameters, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);

			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "UpdatePaymentStatusAsync");
		}

		return false;
	}

	public async ValueTask<OnlineOrderHeader?> GetOnlineOrderHeaderAsync(int orderId)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@orderId", orderId, DbType.Int32);

			return await connection
				.QueryFirstOrDefaultAsync<OnlineOrderHeader>("ord.OnlineOrderGetHeader", dynamicParameters, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetOnlineOrderHeaderAsync");
		}

		return null;
	}

	public async ValueTask<bool> AddStatusToHistoryAsync(int orderId, OrderStatus status, string info, DateTime modifyDate)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@orderId", orderId, DbType.Int32);
			dynamicParameters.Add("@orderStatus", status, DbType.Byte);
			dynamicParameters.Add("@info", info, DbType.String);
			dynamicParameters.Add("@modifyDate", modifyDate, DbType.DateTime);

			await connection
				.ExecuteAsync("ord.OnlineOrderAddStatusToHistory", dynamicParameters, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);

			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "AddStatusToHistoryAsync");
		}

		return false;
	}

	public async ValueTask<OrderPaymentInfo?> GetOrderPaymentInfoAsync(int orderId)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@orderId", orderId, DbType.Int32);

			return await connection
				.QueryFirstOrDefaultAsync<OrderPaymentInfo>("ord.GetOrderPaymentInfo", dynamicParameters, commandType: CommandType.StoredProcedure)
				.ConfigureAwait(false);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetOrderPaymentInfoAsync");
		}

		return null;
	}

	public async ValueTask<CustomerOrderShortInfo[]> GetCustomerOrderShortInfoAsync(int accountId)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@account_id", accountId, DbType.Int32);

			return (await connection.QueryAsync<CustomerOrderShortInfo>(
				"ord.CustomerMyOrders",
				dynamicParameters,
				commandType: CommandType.StoredProcedure)
				).ToArray();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetCustomerOrderShortInfo");
		}

		return Array.Empty<CustomerOrderShortInfo>();
	}
}