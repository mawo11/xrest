using XRest.Restaurants.Contracts;
using MediatR;

namespace XRest.Restaurants.App.Queries.Payment.GetPaymentInformation;

public record GetPaymentInformationQuery(int RestaurantId) : IRequest<PaymentInformation>;