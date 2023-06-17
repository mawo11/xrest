using Dapper;
using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Shared.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Orders.Infrastructure.MsSql;

internal sealed class PaymentRepository : IPaymentRepository
{
	private readonly ISqlConnectionFactory _sqlConnectionFactory;
	private readonly ILogger<PaymentRepository> _logger;

	public PaymentRepository(ISqlConnectionFactory sqlConnectionFactory, ILogger<PaymentRepository> logger)
	{
		_sqlConnectionFactory = sqlConnectionFactory;
		_logger = logger;
	}

	public async ValueTask<bool> SavePaymentHistoryAsync(ChangePaymentHistory[] items)
	{
		try
		{
			using var connection = await _sqlConnectionFactory.MakeConnectionAsync();
			foreach (var item in items)
			{
				await SaveChangePaymentHistory(connection, item);
			}

			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "SavePaymentHistoryAsync");
		}

		return false;
	}
	private static async ValueTask SaveChangePaymentHistory(SqlConnection connection, ChangePaymentHistory changePaymentHistory)
	{
		DynamicParameters dynamicParameters = new();
		dynamicParameters.Add("@date", changePaymentHistory.Date, DbType.DateTime);
		dynamicParameters.Add("@fromPaymentId", changePaymentHistory.FromPaymentId, DbType.Int32);
		dynamicParameters.Add("@toPaymentId", changePaymentHistory.ToPaymentId, DbType.Int32);
		dynamicParameters.Add("@workerId", changePaymentHistory.WorkerId, DbType.Int32);
		dynamicParameters.Add("@billNumber", changePaymentHistory.BillNumber, DbType.Int32);

		await connection.ExecuteAsync(
		   "ord.SavePosPaymentHistory",
		   dynamicParameters,
		   commandType: CommandType.StoredProcedure);
	}
}