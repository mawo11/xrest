using XRest.Consul;
using XRest.Images.App;
using XRest.Images.Infrastructure;
using XRest.Images.Service.Extensions;
using XRest.Shared;
using XRest.Shared.Extensions;
using Microsoft.AspNetCore.Http.Json;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConsulSource();
builder.Configuration.ConfigureNlog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConsul();
builder.Services.AddConsulAgent();
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
builder.Services.AddHealthChecks();
builder.Services.AddInfrastrureServices();
builder.Services.AddSharedServices();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
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

builder.Logging.AddLogging();

var app = builder.Build();

ConsulConfigurationProvider.Register(app.Lifetime);
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.MapHealthChecks("/health");
app.RegisterApiEndpoints(Assembly.GetExecutingAssembly());
app.Run();