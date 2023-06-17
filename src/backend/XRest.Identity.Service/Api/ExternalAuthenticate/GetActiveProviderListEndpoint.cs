using XRest.Identity.App.Queries.ExternalAuthenticate.GetActiveProviderList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.ExternalAuthenticate;

internal static class GetActiveProviderListEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/external/authenticate/platform/{clientType}/active-providers", async (
			 [FromRoute] int clientType,
			 [FromServices] ISender sender) =>
		{
			return await sender.Send(new GetActiveProviderListQuery(clientType));
		})
		 .WithTags("ExternalAuthenticate");
	}
}
