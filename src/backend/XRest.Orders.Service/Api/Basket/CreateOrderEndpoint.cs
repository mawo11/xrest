using XRest.Orders.App.Commands.Basket.CreateOrder;
using XRest.Orders.Contracts.Request.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class CreateOrderEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/create-order", async (
			[FromRoute] string basketKey,
			[FromBody] CreateOrderRequest request,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new CreateOrderCommand(basketKey, request, 0));
		});
	}
}