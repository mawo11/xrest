using XRest.Identity.App.Commands.Customers.SendResetPassword;
using XRest.Identity.Contracts.Customers.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class SendResetPasswordEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/send-reset-password", async (
		   [FromBody] SendResetPasswordRequest request,
		   [FromServices] ISender sender) =>
	   {
		   return await sender.Send(new SendResetPasswordCommand(request.Email));
	   });
	}
}