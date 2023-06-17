using XRest.Orders.Contracts.Responses.Menu;
using MediatR;

namespace XRest.Orders.App.Queries.Menu.GetMenuGroupsForRestaurant;

public record GetMenuGroupsForRestaurantQuery(int RestaurantId, string? Lang) : IRequest<MenuGroup[]>;