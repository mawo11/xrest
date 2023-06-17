using XRest.Identity.App.Commands.ExternalAuthenticate.Login;
using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.ExternalAuthenticate;

internal static class LoginEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/external/authenticate/login", async (
			 HttpContext context,
			 [FromBody] ExternalAuthenticateLogonRequest request,
			 [FromServices] ISender sender) =>
		{
			return await sender.Send(new ExternalLogonCommand(request));
		})
		 .WithTags("ExternalAuthenticate");
	}
}
