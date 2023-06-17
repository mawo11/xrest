	using XRest.Authentication;
using XRest.Orders.App.Commands.Basket.CreateOrder;
using XRest.Orders.Contracts.Request.Basket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class CreateOrderForCustomerEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/basket/{basketKey}/create-order-for-customer", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
			[FromRoute] string basketKey,
			HttpContext context,
			[FromBody] CreateOrderRequest request,
			[FromServices] ISender sender) =>
		{
			var accountId = context.User.FindFirst(CustomClaims.UserId).AsInt();

			return await sender.Send(new CreateOrderCommand(basketKey, request, accountId));
		});
	}
}
