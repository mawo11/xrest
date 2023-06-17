using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.SendResetPassword;

public sealed class SendResetPasswordCommand : IRequest<ResetPasswordResponse>
{
	public SendResetPasswordCommand(string? email)
	{
		Email = email;
	}

	public string? Email { get; }
}
