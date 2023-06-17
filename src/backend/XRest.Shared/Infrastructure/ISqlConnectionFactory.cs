using Microsoft.Data.SqlClient;

namespace XRest.Shared.Infrastructure;

public interface ISqlConnectionFactory
{
	ValueTask<SqlConnection> MakeConnectionAsync();

	SqlConnection MakeConnection();
}
