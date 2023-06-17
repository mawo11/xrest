using XRest.Identity.App.Commands.ExternalAuthenticate.Login;
using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.ExternalAuthenticate;

internal static class ResumeLoginEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/external/authenticate/resume-login", async (
			 HttpContext context,
			 [FromBody] ResumeLoginRequest request,
			 [FromServices] ISender sender) =>
		{
			return await sender.Send(new ResumeLogonCommand(request.Data!, request.CreateNewAccount));
		})
		 .WithTags("ExternalAuthenticate");
	}
}
