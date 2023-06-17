using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace XRest.Shared.Infrastructure;

internal class SqlConnectionFactory : ISqlConnectionFactory
{
	private readonly IConfiguration _configuration;

	public SqlConnectionFactory(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public SqlConnection MakeConnection()
	{
		string connectionString = _configuration.GetValue<string>("ConnectionStrings:db");

		SqlConnection connection = new SqlConnection(connectionString);
		if (!string.IsNullOrEmpty(connectionString))
		{
			connection.Open();
		}

		return connection;
	}

	public async ValueTask<SqlConnection> MakeConnectionAsync()
	{
		string connectionString = _configuration.GetValue<string>("ConnectionStrings:db");

		SqlConnection connection = new SqlConnection(connectionString);
		if (!string.IsNullOrEmpty(connectionString))
		{
			await connection.OpenAsync();
		}

		return connection;
	}
}
