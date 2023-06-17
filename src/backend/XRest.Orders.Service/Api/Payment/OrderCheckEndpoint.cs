using XRest.Orders.App.Queries.Payments.GetOrderPaymentStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class OrderCheckEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/payment/order/{orderId}/check", async
			([FromRoute] int orderId,
			[FromServices] ISender sender) =>
		{

			return await sender.Send(new GetOrderPaymentStatusQuery(orderId));
		});
	}
}
