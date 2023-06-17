using XRest.Orders.Contracts.Common;
using XRest.Orders.Contracts.Responses.FavoriteAddresses;
using MediatR;

namespace XRest.Orders.App.Commands.FavoriteAddresses.SaveFavAddres;

public record SaveFavAddresCommand(int AccountId, FavoriteAddress Item) : IRequest<ApiOperationResult>;
