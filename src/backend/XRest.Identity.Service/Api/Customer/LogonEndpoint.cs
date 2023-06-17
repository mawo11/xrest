using XRest.Identity.App.Commands.Customers.Logon;
using XRest.Identity.Contracts.Customers.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class LogonEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/logon", async (
		   [FromBody] LogonRequest request,
		   [FromServices] ISender sender) =>
	   {
		   return await sender.Send(new LogonCommand(request.Email, request.Password));
	   });
	}
}
