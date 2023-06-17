using XRest.Identity.Contracts.Common;

namespace XRest.Identity.App.Services;

public interface IUserTokenGenerator
{
	ValueTask<TokenData> GenerateAsync(int accountId);
}