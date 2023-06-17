using Dapper;
using XRest.Shared.Domain;
using XRest.Shared.Extensions;
using XRest.Shared.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace XRest.Shared.Services;

internal class MailRepository : IMailRepository
{
	private readonly ISqlConnectionFactory _connection;
	private readonly ILogger<MailRepository> _logger;

	public MailRepository(ISqlConnectionFactory connection, ILogger<MailRepository> logger)
	{
		_connection = connection;
		_logger = logger;
	}

	public async ValueTask<bool> SaveMailAsync(Mail mail)
	{
		try
		{
			using var connection = await _connection.MakeConnectionAsync();
			DynamicParameters dynamicParameters = new();
			dynamicParameters.Add("@id", mail.Id, DbType.Int32);
			dynamicParameters.Add("@status_id", mail.Status, DbType.Byte);
			dynamicParameters.Add("@template_id", mail.Template, DbType.Int32);
			dynamicParameters.Add("@tries", mail.Tries, DbType.Byte);
			dynamicParameters.Add("@address", mail.Address, DbType.String);
			dynamicParameters.Add("@subject", mail.Subject, DbType.String);
			dynamicParameters.Add("@replacement", CustomJsonSerializer.Serialize(mail.Replacements), DbType.String);
			dynamicParameters.Add("@errors", mail.Errors, DbType.String);

			var synchroItems = await connection.ExecuteAsync("[shared].[SaveMail]",
				dynamicParameters,
				commandType: CommandType.StoredProcedure);

			return true;
		}
		catch (Exception e)
		{
			_logger.LogCritical("{message}", e.Message);
			return false;
		}
	}
}
