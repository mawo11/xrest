using XRest.Restaurants.App.Commands.Restaurant.CalculateDeliveryPrice;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Restaurants.Service.Api.Restaurant;

internal static class CalculateDeliveryPriceEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/restaurant/{restaurantId}/calculate-delivery-price", async (
			[FromRoute] int restaurantId,
			[FromBody] CalculateDeliveryPriceRequest request,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new CalculateDeliveryPriceCommand(restaurantId, request.OrderTotal, request.TransportZoneId));
		});
	}
}
