using XRest.Images.App.Domain;
using XRest.Images.App.Queries.GetImage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Images.Service.Api.Images;

internal static class ProductEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/images/product", async ([FromQuery] int fileId, [FromServices] ISender sender) =>
		{
			var result = await sender.Send(new GetImageQuery(ImageType.Product, fileId));
			if (result == null)
			{
				return Results.NotFound();
			}

			return Results.File(result.FullPath, result.Mime, result.FileName);
		});
	}
}
