using XRest.Identity.App.Queries.Terms.GetMarketingAgreement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Terms;

internal static class TermsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/terms", async (
		   [FromQuery] string? lang,
		   [FromServices] ISender sender) =>
	   {
		   return await sender.Send(new GetMarketingAgreementQuery(lang));
	   });
	}
}
