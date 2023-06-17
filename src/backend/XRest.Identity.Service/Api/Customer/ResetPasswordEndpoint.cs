using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.ResetPasswword;
using XRest.Identity.Contracts.Customers.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class ResetPasswordEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/reset-password", async (
			HttpContext context,
		   [FromBody] ResetPasswordRequest request,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.FindFirst(CustomClaims.UserId).AsInt();
		   if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Password))
		   {
			   return Results.BadRequest();
		   }

		   return Results.Ok(await sender.Send(new ResetPasswordCommand(request.Token, request.Password)));
	   });
	}
}
