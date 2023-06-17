using XRest.Orders.App.Commands.Basket.Init;
using XRest.Orders.Contracts.Request.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class InitEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/init/{restaurantId}/{transportZoneId}/{type}", async (
			[FromRoute] int restaurantId,
			[FromRoute] int transportZoneId,
			[FromRoute] DeliveryType type,
			[FromQuery] string basketKey,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new InitCommand(restaurantId, transportZoneId, type, basketKey));
		});
	}
}
