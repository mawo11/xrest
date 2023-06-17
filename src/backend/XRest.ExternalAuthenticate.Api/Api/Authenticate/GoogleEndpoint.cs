using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using XRest.Identity.Clients;

namespace XRest.ExternalAuthenticate.Api.Api.Authenticate;

internal static class GoogleEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/authenticate/google", async (
			HttpRequest request,
			HttpResponse response,
			[FromServices] ILogger<Object> logger,
			[FromServices] IConfiguration configuration,
			[FromServices] IExternalAuthenticateService externalAuthenticateService,
			[FromQuery] ClientType clientType) =>
	   {
		   var authScheme = GoogleDefaults.AuthenticationScheme;

		   var authResult = await request.HttpContext.AuthenticateAsync(authScheme);
		   if (!authResult.Succeeded
			   || authResult?.Principal == null
			   || !authResult.Principal.Identities.Any(id => id.IsAuthenticated)
			   || string.IsNullOrEmpty(authResult.Properties.GetTokenValue("access_token")))
		   {
			   logger.Log(LogLevel.Debug, "Brak autoryzycji przez: {authScheme}. Wymiana...", authScheme);

			   var externalAuthenticateAppUrl = configuration.GetValue<string>("ExternalAuthenticate:ExternalAuthenticateAppUrl");

			   await request.HttpContext.ChallengeAsync(authScheme, new AuthenticationProperties
			   {
				   AllowRefresh = true,
				   ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
				   IssuedUtc = DateTimeOffset.UtcNow,
				   RedirectUri = $"{externalAuthenticateAppUrl.TrimEnd('/')}/authenticate/google?clienttype={(int)clientType}"
			   });

			   if (response.Headers.TryGetValue("location", out var locationResponseHeader))
			   {
				   logger.Log(LogLevel.Debug, "Brak autoryzacji przez: {authScheme}. Przekierowanie do: {locationResponseHeader}.",
					   authScheme,
					   locationResponseHeader);
				   return Results.Redirect(locationResponseHeader);
			   }

			   logger.Log(LogLevel.Debug, "Autoryzacji przez: {authScheme} nie udana", authScheme);
			   return Results.Unauthorized();
		   }

		   var claimsIdentity = authResult.Principal.Identities.First(id => id.IsAuthenticated);
		   var email = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
		   var nameIdentifier = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
		   var givenName = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.GivenName)?.Value;
		   var surname = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Surname)?.Value;
		   logger.Log(LogLevel.Debug, "Uzytkownik {claimsIdentity.Name} z email {email } autoryzowany przez {authScheme}.",
			   claimsIdentity.Name,
			   email ?? string.Empty,
			   authScheme);

		   var result = await externalAuthenticateService.Login(new Identity.Contracts.ExternalAuthenticate.Request.ExternalAuthenticateLogonRequest
		   {
			   Email = email,
			   ExternalIdentifier = nameIdentifier,
			   Firstname = givenName,
			   Surname = surname,
			   ClientType = clientType,
			   ExternalProvider = ExternalProviderType.Google
		   });

		   await request.HttpContext.SignOutAsync();

		   if (result.Status == Consul.ServiceCallStatus.Ok && result.Data != null && result.Data.Success)
		   {
			   return Results.Redirect(result.Data.Url!);
		   }

		   logger.LogError("Cos poszlo nie tak...");

		   return Results.BadRequest();
	   });
	}
}

