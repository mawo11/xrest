using XRest.Orders.App.Queries.Menu.MenuGetProductDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Menu;

internal static class ProductDetailsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/menu/product/{productId}/details", async (
			[FromRoute] int productId,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new MenuGetProductDetailsQuery(productId, lang));
		});
	}
}
