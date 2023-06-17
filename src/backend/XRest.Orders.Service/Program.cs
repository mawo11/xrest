using XRest.Authentication;
using XRest.Consul;
using XRest.Identity.Contracts;
using XRest.Identity.Contracts.Customers;
using XRest.Orders.App;
using XRest.Orders.Infrastructure;
using XRest.Orders.Service.Extensions;
using XRest.Restaurants.Contracts;
using XRest.Shared;
using XRest.Shared.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using XRest.Orders.Service.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConsulSource();
builder.Configuration.ConfigureNlog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "XRest.Orders.Service", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Proszê podaæ token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddConsul();
builder.Services.AddConsulAgent();
builder.Services.AddHealthChecks();
builder.Services.AddServiceClient<IRestaurantService>(RestaurantsServiceName.Name);
builder.Services.AddServiceClient<IPaymentInformationService>(RestaurantsServiceName.Name);
builder.Services.AddServiceClient<ICustomerService>(IdentityServiceName.Name);
builder.Services.AddHostedService<ReadOnlyRepositoryHostedService>();
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
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Logging.AddLogging();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapHealthChecks("/health");
app.UseDeveloperExceptionPage();
app.RegisterApiEndpoints(Assembly.GetExecutingAssembly());
app.Run();