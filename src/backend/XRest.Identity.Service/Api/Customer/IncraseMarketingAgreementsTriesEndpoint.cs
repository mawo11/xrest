using XRest.Authentication;
using XRest.Identity.App.Commands.Customers.IncreaseCustomerMarketingAgreementTries;
using XRest.Identity.Contracts.Customers.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace XRest.Identity.Service.Api.Customer;

internal static class IncraseMarketingAgreementsTriesEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/customer/incrase-marketing-agreements-tries", [Authorize(AuthenticationSchemes = SecuritySchemes.UserAccessScheme, Policy = PolicyTypes.UserAccessPolicy)] async (
		   HttpContext context,
		   [FromBody] IEnumerable<CustomerMarketingAgreement> items,
		   [FromServices] ISender sender) =>
	   {
		   var accountId = context.User.GetUserId();

		   await sender.Send(new IncreaseCustomerMarketingAgreementTriesCommand(accountId));
	   });
	}
}
