using MediatR;

namespace XRest.Restaurants.App.Queries.Payment.GetPaymentsForOrdering;

public record GetPaymentsForOrderingQuery(int RestaurantId, string Lang) : IRequest<Contracts.Payment[]>;