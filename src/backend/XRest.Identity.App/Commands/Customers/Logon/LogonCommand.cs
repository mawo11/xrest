using XRest.Identity.Contracts.Customers.Responses;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.Logon;

public record LogonCommand(string? Email, string? Password) : IRequest<LogonData>;