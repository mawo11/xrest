using XRest.Orders.App.Commands.Basket.ChangeDeliveryType;
using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ChangeDeliveryTypeEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/change-delivery-type/{deliveryType}", async (
			[FromRoute] string basketKey,
			[FromRoute] DeliveryType deliveryType,
			[FromServices] ISender sender) =>
		{
			return new OperationResult
			{
				Status = await sender.Send(new ChangeDeliveryTypeCommand(basketKey, deliveryType))
			};
		});
	}
}