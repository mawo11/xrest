using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.ExternalAuthenticate.GetLogonDataByLoginId;

public record GetLogonDataByLoginIdCommand(string? LogonId) : IRequest<LogonData>;