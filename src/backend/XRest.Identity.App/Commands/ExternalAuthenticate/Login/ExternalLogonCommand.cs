using XRest.Identity.Contracts.ExternalAuthenticate.Request;
using XRest.Identity.Contracts.ExternalAuthenticate.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.ExternalAuthenticate.Login;

public record ExternalLogonCommand(ExternalAuthenticateLogonRequest Request) : IRequest<ExternalLoginResponse>;