using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Common;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Identity.App.Commands.Customers.SaveCustomerMakertingAgreements;

public sealed class SaveCustomerMakertingAgreementsCommandHandler : IRequestHandler<SaveCustomerMakertingAgreementsCommand, ApiOperationResult>
{
	private readonly static ApiOperationResult Ok = new(ApiOperationResultStatus.Ok);

	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<SaveCustomerMakertingAgreementsCommandHandler> _logger;

	public SaveCustomerMakertingAgreementsCommandHandler(IAccountRepository accountRepository, ILogger<SaveCustomerMakertingAgreementsCommandHandler> logger)
	{
		_accountRepository = accountRepository;
		_logger = logger;
	}

	public async Task<ApiOperationResult> Handle(SaveCustomerMakertingAgreementsCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("SaveCustomerMakertingAgreementsCommandHandler {data}", CustomJsonSerializer.Serialize(request));

		await _accountRepository.RemoveAllCustomerMarketingAgreementAsync(request.AccountId);
		int allAgreements = 0;
		int allowedAgreements = 0;

		foreach (var item in request.Items)
		{
			allAgreements++;
			if (!item.Checked) continue;
			allowedAgreements++;
			await _accountRepository.AddCustomerMarketingAgreementAsync(request.AccountId, item.Id);
		}

		if (allAgreements == allowedAgreements)
		{
			await _accountRepository.DisableAgreementCounterAsync(request.AccountId);
		}

		return Ok;
	}
}
