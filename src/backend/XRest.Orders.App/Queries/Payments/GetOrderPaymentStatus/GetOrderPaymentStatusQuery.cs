using XRest.Orders.Contracts.Responses.Payments;
using MediatR;

namespace XRest.Orders.App.Queries.Payments.GetOrderPaymentStatus;

public record GetOrderPaymentStatusQuery(int OrderId) : IRequest<OrderPaymentStatus>;
