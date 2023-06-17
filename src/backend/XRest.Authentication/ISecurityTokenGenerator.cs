using System.Security.Claims;

namespace XRest.Authentication;

public interface ISecurityTokenGenerator
{
	string GenerateToken(int expiryInMinutes, string audience, params Claim[] claims);
}
