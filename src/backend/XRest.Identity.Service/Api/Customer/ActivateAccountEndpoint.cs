using XRest.Identity.App.Commands.Customers.ActivateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class ActivateAccountEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/{token}/activate", async (
		   HttpContext context,
		   [FromRoute] string token,
		   [FromServices] ISender sender) =>
	   {
		   return await sender.Send(new ActivateAccountCommand(token));
	   });
	}
}