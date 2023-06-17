using MediatR;
using XRest.Identity.Contracts.Customers.Responses;
using XRest.Identity.App.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using XRest.Shared.Extensions;

namespace XRest.Identity.App.Queries.Customers.GetCustomerMarketingAgreements;

public sealed class GetCustomerMarketingAgreementsQueryHandler : IRequestHandler<GetCustomerMarketingAgreementsQuery, IEnumerable<CustomerMarketingAgreement>>
{
	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<GetCustomerMarketingAgreementsQueryHandler> _logger;

	public GetCustomerMarketingAgreementsQueryHandler(IAccountRepository accountRepository, ILogger<GetCustomerMarketingAgreementsQueryHandler> logger)
	{
		_accountRepository = accountRepository;
		_logger = logger;
	}

	public async Task<IEnumerable<CustomerMarketingAgreement>> Handle(GetCustomerMarketingAgreementsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetCustomerPointHistoryQueryHandler => {data} ", JsonSerializer.Serialize(request));

		var items = await _accountRepository.GetMarketingAgreementAsync(request.AccountId);

		return items
			.Where(static x => !string.IsNullOrEmpty(x.Content))
			.Select( x => new CustomerMarketingAgreement
			{
				Checked = x.Checked,
				Content = x.Content!.GetTranslate(request.Lang, x.Content!),
				Id = x.Id,
			}).ToList();

	}
}
