using Dapper;
using XRest.Images.App.Domain;
using XRest.Images.App.Services;
using XRest.Shared.Infrastructure;
using System.Data;

namespace XRest.Images.Infrastructure.MsSql;

public class ImageRepository : IImageRepository
{
	private readonly ISqlConnectionFactory _connectionFactory;

	public ImageRepository(ISqlConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async ValueTask<string> GetImageMimeAsync(int imageId, ImageType imageType)
	{
		using var connection = await _connectionFactory.MakeConnectionAsync();
		DynamicParameters dynamicParameters = new DynamicParameters();
		dynamicParameters.Add("@itemId", imageId, DbType.Int32);
		dynamicParameters.Add("@itemGroup", imageType, DbType.Byte);

		return await connection.ExecuteScalarAsync<string>(
			  "rest.GetImage",
			  dynamicParameters,
			  commandType: CommandType.StoredProcedure);
	}
}
