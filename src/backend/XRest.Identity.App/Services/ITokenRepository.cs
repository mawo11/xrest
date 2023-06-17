using XRest.Identity.App.Domain;

namespace XRest.Identity.App.Services;

public interface ITokenRepository
{
	ValueTask<RefreshTokenData> GetByTokenAsync(string token);

	ValueTask RemoveByIdAsync(int id);

	ValueTask AddNewAsync(RefreshTokenData refreshTokenData);
}
