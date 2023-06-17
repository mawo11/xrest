using XRest.Orders.App.Commands.Payments.ConfirmPayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Orders.Service.Api.Payment;

internal static class ConfirmEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/payment/confirm", async
			(HttpRequest request,
			[FromServices] ISender sender) =>
		{
			string orderId = request.Form["p24_session_id"].ToString(); // "pos_order_id
			string paymentOrderId = request.Form["p24_order_id"].ToString(); // Numer transakcji nadany przez Przelewy24
			string method = request.Form["p24_method"].ToString();
			string statement = request.Form["p24_statement"].ToString();

			return await sender.Send(new ConfirmPaymentCommand(orderId, paymentOrderId, method, statement));
		});
	}
}
