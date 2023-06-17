using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Responses;
using MediatR;
using XRest.Shared.Extensions;

namespace XRest.Identity.App.Queries.Terms.GetMarketingAgreement;

public sealed class GetMarketingAgreementQueryHandler : IRequestHandler<GetMarketingAgreementQuery, MarketingAgreement[]>
{
	private readonly IMarketingAgreementRepository _marketingAgreementRepository;

	public GetMarketingAgreementQueryHandler(IMarketingAgreementRepository marketingAgreementRepository)
	{
		_marketingAgreementRepository = marketingAgreementRepository;
	}

	public async Task<MarketingAgreement[]> Handle(GetMarketingAgreementQuery request, CancellationToken cancellationToken)
	{
		var result = await _marketingAgreementRepository.GetMarketingAgreementsAsync();

		return result
			.Where(static x => !string.IsNullOrEmpty(x.Content))
			.Select(x => new MarketingAgreement
			{
				Id = x.Id,
				Content = x.Content!.GetTranslate(request.Lang, x.Content!)
			})
			.ToArray();
	}
}
