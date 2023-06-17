using XRest.Orders.App.Queries.Basket.GetBasketView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ViewEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/basket/{basketKey}/view", async (
			[FromRoute] string basketKey,
			[FromQuery] string? lang,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new GetBasketViewQuery(basketKey, lang));
		});
	}
}
