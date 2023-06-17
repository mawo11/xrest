using Microsoft.AspNetCore.Mvc;
using XRest.Identity.Clients;
using XRest.ExternalAuthenticate.Api.Data;

namespace XRest.ExternalAuthenticate.Api.Api.Authenticate;
internal static class AnswerEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapPost("/authenticate/answer", async (
			[FromServices] ILogger<Object> logger,
			[FromBody] AnswerRequest request,
			[FromServices] IExternalAuthenticateService externalAuthenticateService) =>
	   {

		   if (string.IsNullOrEmpty(request.Data) || string.IsNullOrEmpty(request.Answer) || !(request.Answer == "y" || request.Answer == "n"))
		   {
			   logger.LogError("Brak danych: data({Data}), answer:{Answer}", request.Data, request.Answer);
			   return Results.BadRequest();
		   }

		   var result = await externalAuthenticateService.ResumeLogin(new Identity.Contracts.ExternalAuthenticate.Request.ResumeLoginRequest
		   {
			   CreateNewAccount = request.Answer == "y",
			   Data = request.Data
		   });

		   if (result.Status == Consul.ServiceCallStatus.Ok && result.Data != null && result.Data.Success)
		   {
			   return Results.Ok(result.Data.Url!);
		   }

		   return Results.BadRequest();
	   });
	}
}

