using XRest.Orders.Contracts.Responses.Menu;
using MediatR;

namespace XRest.Orders.App.Queries.Menu.GetMenuGroupProductsForRestauantAndGroup;

public record GetMenuGroupProductsForRestauantAndGroupQuery(int RestaurantId, int GroupId, string? Lang) : IRequest<MenuProduct[]>;