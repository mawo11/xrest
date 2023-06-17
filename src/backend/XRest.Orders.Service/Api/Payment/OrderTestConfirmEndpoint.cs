using XRest.Orders.App.Commands.Payments.ConfirmPayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class OrderTestConfirmEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/payment/order/{orderId}/test-confirm", async
			([FromRoute] string orderId,
			[FromServices] ISender sender) =>
		{
			return await sender.Send(new ConfirmPaymentCommand(orderId, orderId, "Test", "!!! Testowe zatwierdzenie !!!"));
		});
	}
}
