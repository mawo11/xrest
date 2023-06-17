using XRest.Identity.Contracts.Common;
using MediatR;

namespace XRest.Identity.App.Commands.Customers.RefreshToken;

public record RefreshTokenCommand(int AccountId, string Token) : IRequest<TokenData>;