using XRest.Authentication;
using XRest.Orders.App.Queries.Customers.GetMyOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class MyOrdersEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/customer/my-orders", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async
			(HttpRequest request,
			 HttpContext context,
			[FromServices] ISender sender) =>
		{
			var accountId = context.User.FindFirst(CustomClaims.UserId).AsInt();
			return await sender.Send(new GetMyOrdersQuery(accountId));
		});
	}
}
