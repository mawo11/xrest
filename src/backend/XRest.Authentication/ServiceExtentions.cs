using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace XRest.Authentication;

public static class ServiceExtentions
{
	public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<ISecurityTokenGenerator, SecurityTokenGenerator>();

		string tokenKey = configuration.GetValue<string>("Identity:TokenKey");
		if (string.IsNullOrEmpty(tokenKey))
		{
			NLog.LogManager.GetLogger("*").Error("brak tokenu");
			StringBuilder stringBuilder = new StringBuilder();
			foreach (var item in configuration.AsEnumerable())
			{
				stringBuilder.AppendLine($"{item.Key}:{item.Value}");
			}

			NLog.LogManager.GetLogger("*").Error(stringBuilder);
			return;
		}

		string[] tokenKeyParts = tokenKey.Split('.');

		var ecKeyTemp = Encoding.UTF8.GetBytes(tokenKeyParts[1]);
		byte[] ecKey = new byte[256 / 8];
		Array.Copy(ecKeyTemp, ecKey, 256 / 8);

		var secretKey = new SymmetricSecurityKey(ecKey);

		services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
	   .AddJwtBearer(SecuritySchemes.RefreshScheme, jwtBearerOptions => Configure(jwtBearerOptions, SecuritySchemes.RefreshScheme, secretKey))
	   .AddJwtBearer(SecuritySchemes.UserAccessScheme, jwtBearerOptions => Configure(jwtBearerOptions, SecuritySchemes.UserAccessScheme, secretKey))
	   .AddJwtBearer(SecuritySchemes.SystemAccessScheme, jwtBearerOptions => Configure(jwtBearerOptions, SecuritySchemes.SystemAccessScheme, secretKey))
	   .AddJwtBearer(SecuritySchemes.AppAccessScheme, jwtBearerOptions => Configure(jwtBearerOptions, SecuritySchemes.AppAccessScheme, secretKey));


		services.AddAuthorization(options =>
		{
			options.AddPolicy(PolicyTypes.UserAccessPolicy, policy =>
			{
				policy.RequireAuthenticatedUser();
				policy.RequireClaim(CustomClaims.Audience, SecuritySchemes.UserAccessScheme);
			});

			options.AddPolicy(PolicyTypes.SystemResetPasswordPolicy, policy =>
			{
				policy.RequireClaim(CustomClaims.Audience, SecuritySchemes.SystemAccessScheme);
				policy.RequireClaim(CustomClaims.SystemOperation, SysOperations.ResetPassword);
			});

			options.AddPolicy(PolicyTypes.SystemAccountConfirmPolicy, policy =>
			{
				policy.RequireClaim(CustomClaims.Audience, SecuritySchemes.SystemAccessScheme);
				policy.RequireClaim(CustomClaims.SystemOperation, SysOperations.ActivateAccount);
			});

			options.AddPolicy(PolicyTypes.AppAccessPolicy, policy =>
			{
				policy.RequireClaim(CustomClaims.Audience, SecuritySchemes.AppAccessScheme);
			});
		});
	}

	private static JwtBearerOptions Configure(JwtBearerOptions jwtBearerOptions, string audience, SymmetricSecurityKey securityKey)
	{
		jwtBearerOptions.RequireHttpsMetadata = false;
		jwtBearerOptions.SaveToken = true;
		jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
		{
			ValidAudience = audience,
			ValidIssuer = Issuer.Name,
			RequireSignedTokens = false,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero,
			TokenDecryptionKey = securityKey
		};

		return jwtBearerOptions;
	}
}
