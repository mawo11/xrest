using XRest.Authentication;
using XRest.Identity.App.Domain;
using XRest.Identity.Contracts.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using XRest.Shared.Services;

namespace XRest.Identity.App.Services;

public sealed class UserTokenGenerator : IUserTokenGenerator
{
	private readonly IConfiguration _configuration;
	private readonly ISecurityTokenGenerator _securityTokenGenerator;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ITokenRepository _tokenRepository;
	private readonly ILogger<UserTokenGenerator> _logger;

	public UserTokenGenerator(IConfiguration configuration,
		ISecurityTokenGenerator securityTokenGenerator,
		IDateTimeProvider dateTimeProvider,
		ITokenRepository tokenRepository,
		ILogger<UserTokenGenerator> logger)
	{
		_logger = logger;
		_configuration = configuration;
		_securityTokenGenerator = securityTokenGenerator;
		_dateTimeProvider = dateTimeProvider;
		_tokenRepository = tokenRepository;
	}

	public async ValueTask<TokenData> GenerateAsync(int accountId)
	{
		int logonTokenValidMinutes = _configuration.GetValue<int>("Identity:LogonTokenValidMinutes");
		_logger.LogInformation("logonTokenValidMinutes: {logonTokenValidMinutes}", logonTokenValidMinutes);

		string accessToken = _securityTokenGenerator.GenerateToken(logonTokenValidMinutes,
			  SecuritySchemes.UserAccessScheme,
			  new Claim(CustomClaims.UserId, accountId.ToString()));

		var tokenData = new RefreshTokenData
		{
			Created = _dateTimeProvider.Now,
			Expired = _dateTimeProvider.Now.AddMinutes(logonTokenValidMinutes + 1),
			Token = Guid.NewGuid().ToString()
		};

		await _tokenRepository.AddNewAsync(tokenData);

		string refreshToken = _securityTokenGenerator.GenerateToken(logonTokenValidMinutes,
			 SecuritySchemes.RefreshScheme,
			 new Claim(CustomClaims.UserId, accountId.ToString()),
			 new Claim(CustomClaims.RefreshToken, tokenData.Token));

		return new TokenData
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken,
			ExpiresIn = logonTokenValidMinutes
		};
	}
}
