using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.ExternalAuthenticate.Login;

public record ResumeLogonCommand(string Data, bool CreateNewAccount) : IRequest<ExternalLoginResponse>;