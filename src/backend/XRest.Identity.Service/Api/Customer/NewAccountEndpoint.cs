using XRest.Identity.App.Commands.Customers.NewAccount;
using XRest.Identity.Contracts.Customers.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class NewAccountEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/new-account", async (
		   [FromBody] NewAccountRequest newAccount,
		   [FromServices] ISender sender) =>
	   {
		   return await sender.Send(new NewAccountCommand(newAccount));
	   });
	}
}
