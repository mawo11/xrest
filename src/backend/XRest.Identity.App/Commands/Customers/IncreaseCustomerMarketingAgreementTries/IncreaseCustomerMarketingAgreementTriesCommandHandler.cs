using XRest.Identity.App.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using XRest.Shared.Extensions;

namespace XRest.Identity.App.Commands.Customers.IncreaseCustomerMarketingAgreementTries;

public sealed class IncreaseCustomerMarketingAgreementTriesCommandHandler : IRequestHandler<IncreaseCustomerMarketingAgreementTriesCommand, bool>
{
	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<IncreaseCustomerMarketingAgreementTriesCommandHandler> _logger;

	public IncreaseCustomerMarketingAgreementTriesCommandHandler(IAccountRepository accountRepository,
		ILogger<IncreaseCustomerMarketingAgreementTriesCommandHandler> logger)
	{
		_accountRepository = accountRepository;
		_logger = logger;
	}

	public async Task<bool> Handle(IncreaseCustomerMarketingAgreementTriesCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("IncreaseCustomerMarketingAgreementTriesCommandHandler: {request}", CustomJsonSerializer.Serialize(request));
		await _accountRepository.IncreaseCustomerMarketingAgreementTriesAsync(request.AccountId);

		return true;
	}
}
