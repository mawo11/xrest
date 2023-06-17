using XRest.Identity.Contracts.Responses;

namespace XRest.Identity.App.Services;

public interface IMarketingAgreementRepository
{
	ValueTask<MarketingAgreement[]> GetMarketingAgreementsAsync();
}
