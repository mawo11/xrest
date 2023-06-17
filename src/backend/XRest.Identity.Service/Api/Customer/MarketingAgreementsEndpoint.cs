using XRest.Authentication;
using XRest.Identity.App.Queries.Customers.GetCustomerMarketingAgreements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class MarketingAgreementsEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/customer/marketing-agreements", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromQuery] string? lang,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();

		   return await sender.Send(new GetCustomerMarketingAgreementsQuery(accountId, lang));
	   });
	}
}
