using XRest.Identity.App.Commands.ExternalAuthenticate.GetLogonDataByLoginId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.ExternalAuthenticate;

internal static class GetLogonDataByLoginIdEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/external/authenticate/{logonId}/logon-data", async (
			 string logonId,
			 [FromServices] ISender sender) =>
		{
			return await sender.Send(new GetLogonDataByLoginIdCommand(logonId));
		})
		 .WithTags("ExternalAuthenticate");
	}
}
