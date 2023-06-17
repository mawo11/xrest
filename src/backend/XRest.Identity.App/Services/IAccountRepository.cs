using XRest.Identity.App.Domain;

namespace XRest.Identity.App.Services;

public interface IAccountRepository
{
	ValueTask<int> InsertNewAccountAsync(NewAccountData newAccountData);

	ValueTask AddPointsAsync(int userId, int points, string comment, DateTime creation, int? orderId = null);

	ValueTask SetAccountStatusAsync(int accountId, AccountStatus status);

	ValueTask<Account?> GetAccountByEmailAsync(string email);

	ValueTask<Account?> GetAccountByIdAsync(int accountId);

	ValueTask<Account?> GetAccountByExternalIdAsync(ExternalProvider exernalProvider, string externalId);

	ValueTask UpdateAccountPasswordAsync(int accountId, string password);

	ValueTask SaveAccountAsync(Account account);

	ValueTask DisableAccountAsync(int accountId);

	ValueTask<IEnumerable<CustomerPoint>> GetAccountPointsHistoryAsync(int accountId);

	ValueTask IncreaseCustomerMarketingAgreementTriesAsync(int accountId);

	ValueTask<IEnumerable<MarketingAgreement>> GetMarketingAgreementAsync(int accountId);

	ValueTask AddCustomerMarketingAgreementAsync(int acccoaccountIduId, int marketingId);

	ValueTask RemoveAllCustomerMarketingAgreementAsync(int accountId);

	ValueTask DisableAgreementCounterAsync(int accountId);

	ValueTask<bool> AddNewLoginMethodToAccountAsync(int accountId, string externalId, ExternalProvider externalProvider);
}
