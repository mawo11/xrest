using XRest.Identity.App.Domain;

namespace XRest.Identity.App.Services;

public interface IAccountOperationRepository
{
	ValueTask<bool> AddAccountOperationAsync(AccountOperation accountOperation);

	ValueTask<AccountOperation> GetAccountOperationByTokenAsync(string token);

	ValueTask<bool> RemoveAccountOperationByTokenAsync(string token);
}
