using XRest.Orders.Contracts.Common;
using MediatR;

namespace XRest.Orders.App.Commands.FavoriteAddresses.DeleteFavAddres;

public record DeleteFavAddresCommand(int AccountId, int Id) : IRequest<ApiOperationResult>;