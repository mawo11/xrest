using XRest.Consul;
using XRest.Shared.Extensions;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using NLog.Web;
using XRest.ExternalAuthenticate.Api.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using XRest.ExternalAuthenticate.Api.Api.Authenticate;
using XRest.Identity.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConsulSource();
builder.Configuration.ConfigureNlog();

builder.Services
	.AddAuthentication(options =>
		{
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		})
	.AddCookie()
	.AddFacebook(facebookOptions =>
	{
		facebookOptions.AppId = builder.Configuration.GetValue<string>("ExternalAuthenticate:Facebook:AppId");
		facebookOptions.AppSecret = builder.Configuration.GetValue<string>("ExternalAuthenticate:Facebook:Secret");
		facebookOptions.SaveTokens = true;
	}).AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = builder.Configuration.GetValue<string>("ExternalAuthenticate:Google:AppId");
		googleOptions.ClientSecret = builder.Configuration.GetValue<string>("ExternalAuthenticate:Google:Secret");
		googleOptions.SaveTokens = true;
	});
builder.Services.AddConsul();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<XRest.Identity.Clients.IExternalAuthenticateService, ExternalAuthenticateServiceClient>();
builder.Services.Configure<JsonOptions>(options =>
{
	options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
	options.SerializerOptions.PropertyNameCaseInsensitive = true;
	options.SerializerOptions.AllowTrailingCommas = true;
	options.SerializerOptions.MaxDepth = 64;
	options.SerializerOptions.IncludeFields = true;
	options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
	options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
	options.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", p =>
	{
		p.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod();
	});
});
builder.Services.AddAuthorization();
builder.Services.AddControllers()
		   .AddJsonOptions(options =>
		   {
			   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		   });
builder.Logging.AddNLogWeb();

var app = builder.Build();


app.UseCors("AllowAll");

app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Lax,
});

app.UseAuthentication();
app.UseAuthorization();

app.RegisterApiEndpoints(typeof(FacebookEndpoint).Assembly);
app.UseStaticFiles();

app.Run();