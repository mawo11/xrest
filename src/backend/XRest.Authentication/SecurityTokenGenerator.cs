using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace XRest.Authentication;

public sealed class SecurityTokenGenerator : ISecurityTokenGenerator
{
	private readonly SymmetricSecurityKey _secretKey;

	public SecurityTokenGenerator(IConfiguration configuration)
	{
		string tokenKey = configuration.GetValue<string>("Identity:TokenKey");
		string[] tokenKeyParts = tokenKey.Split('.');


		var ecKeyTemp = Encoding.UTF8.GetBytes(tokenKeyParts[1]);
		byte[] ecKey = new byte[256 / 8];
		Array.Copy(ecKeyTemp, ecKey, 256 / 8);

		_secretKey = new SymmetricSecurityKey(ecKey);
	}

	public string GenerateToken(int expiryInMinutes, string audience, params Claim[] claims)
	{
		var handler = new JwtSecurityTokenHandler();

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
			Audience = audience,
			Issuer = Issuer.Name,
			Subject = new ClaimsIdentity(claims),
			IssuedAt = DateTime.UtcNow,
			EncryptingCredentials = new EncryptingCredentials(_secretKey, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512)
		};

		return handler.CreateEncodedJwt(tokenDescriptor);
	}
}
