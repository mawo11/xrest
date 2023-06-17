using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Common;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.SaveDetails;

public sealed class SaveDetailsCommandHandler : IRequestHandler<SaveDetailsCommand, ApiOperationResult>
{
	private readonly static ApiOperationResult Error = new(ApiOperationResultStatus.Error);
	private readonly static ApiOperationResult Ok = new(ApiOperationResultStatus.Ok);

	private readonly IAccountRepository _accountRepository;

	public SaveDetailsCommandHandler(IAccountRepository accountRepository)
	{
		_accountRepository = accountRepository;
	}

	public async Task<ApiOperationResult> Handle(SaveDetailsCommand request, CancellationToken cancellationToken)
	{
		var user = await _accountRepository.GetAccountByIdAsync(request.AccountId);
		if (user == null)
		{
			return Error;
		}

		user.Firstname = request.MyData.Firstname;
		user.Lastname = request.MyData.Lastname;
		user.BirthDate = request.MyData.Birthdate;
		user.Phone = request.MyData.Phone;

		await _accountRepository.SaveAccountAsync(user);

		return Ok;
	}
}
