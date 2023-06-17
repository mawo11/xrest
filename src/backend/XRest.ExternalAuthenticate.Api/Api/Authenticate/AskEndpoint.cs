using Microsoft.AspNetCore.Mvc;
using XRest.Identity.Clients;

namespace XRest.ExternalAuthenticate.Api.Api.Authenticate;
internal static class AskEndpoint
{
	internal static void Register(WebApplication applicationBuilder)
	{
		applicationBuilder.MapGet("/authenticate/ask", async (
			[FromQuery] string? data,
			[FromQuery] string? provider,
			[FromQuery] string? email,
			[FromServices] IWebHostEnvironment hostEnvironment,
			[FromServices] IExternalAuthenticateService externalAuthenticateService) =>
	   {
		   if (string.IsNullOrEmpty(data))
		   {
			   return Results.BadRequest();
		   }

		   string folder = hostEnvironment.WebRootPath;
		   string file = Path.Combine(folder, "ask.html");
		   if (!File.Exists(file))
		   {
			   return Results.BadRequest();
		   }

		   string content = await File.ReadAllTextAsync(file);
		   content = content.Replace("{data}", data);
		   content = content.Replace("{provider}", provider);
		   content = content.Replace("{email}", email);

		   return Results.Content(content, "text/html");
	   });
	}
}

