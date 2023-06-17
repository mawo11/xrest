using MediatR;

namespace XRest.Orders.App.Commands.Payments.ConfirmPayment;

public record ConfirmPaymentCommand(string OrderId, string PaymentOrderId, string Method, string Statement) : IRequest<string>;