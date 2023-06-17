using Dapper;
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Orders.Infrastructure.MsSql;

internal sealed class OnlineOrderInvoiceRepository : IOnlineOrderInvoiceRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<OnlineOrderInvoiceRepository> _logger;

	public OnlineOrderInvoiceRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<OnlineOrderInvoiceRepository> logger)
	{
		_sqlConnectionFactory = sqlConnectionFactory;
		_logger = logger;
	}

	public async ValueTask<NewInvoiceNumberResult> GetNextInvoiceNumberAsync(int restaurantId, int year, int month)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@restaurantId", restaurantId, DbType.Int32);
			dynamicParameters.Add("@year", year, DbType.Int32);
			dynamicParameters.Add("@month", month, DbType.Int32);

			return await connection.QueryFirstAsync<NewInvoiceNumberResult>("ord.GetNextInvoiceNumber", dynamicParameters,
				commandType: CommandType.StoredProcedure);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "GetNextInvoiceNumber");
			return NewInvoiceNumberResult.Invalid;
		}
	}

	public async ValueTask<bool> SaveInvoiceAsync(OnlineInvoice onlineInvoice)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@orderId", onlineInvoice.OrderId, DbType.Int32);
			dynamicParameters.Add("@nip", onlineInvoice.Nip, DbType.String);
			dynamicParameters.Add("@address", onlineInvoice.Address, DbType.String);
			dynamicParameters.Add("@name", onlineInvoice.Name, DbType.String);
			dynamicParameters.Add("@number", onlineInvoice.Number, DbType.String);
			dynamicParameters.Add("@createDate", onlineInvoice.CreateDate, DbType.DateTime);

			await connection.QueryFirstAsync<int>("ord.CreateInvoice", dynamicParameters, commandType: CommandType.StoredProcedure);
			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "CreateInvoice");
			return false;
		}
	}
}
