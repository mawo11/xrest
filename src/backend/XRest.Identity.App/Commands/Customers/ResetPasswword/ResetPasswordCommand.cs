using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.ResetPasswword;

public record ResetPasswordCommand(string Token, string? Password) : IRequest<ResetPasswordResponse>;