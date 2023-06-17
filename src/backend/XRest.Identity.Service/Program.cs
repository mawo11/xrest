using XRest.Authentication;
using XRest.Consul;
using XRest.Identity.App;
using XRest.Identity.Infratructure;
using XRest.Identity.Service.Extensions;
using XRest.Restaurants.Contracts;
using XRest.Shared;
using XRest.Shared.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConsulSource();
builder.Configuration.ConfigureNlog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "XRest.Identity.Service", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Proszê podaæ token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});

});
builder.Services.AddConsul();
builder.Services.AddConsulAgent();
builder.Services.AddHealthChecks();
builder.Services.AddDataProtection();
builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(typeof(AppMarker).Assembly);
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
builder.Services.AddAppServices();
builder.Services.AddInfrastrureServices();
builder.Services.AddSharedServices();
builder.Services.AddMemoryCache();
builder.Services.AddPolicyRegistry();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServiceClient<IRestaurantService>(RestaurantsServiceName.Name);
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
builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Logging.AddLogging();

var app = builder.Build();

ConsulConfigurationProvider.Register(app.Lifetime);
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapHealthChecks("/health");

app.RegisterApiEndpoints(Assembly.GetExecutingAssembly());
app.Run();