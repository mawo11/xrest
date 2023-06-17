using RestEase;

namespace XRest.Restaurants.Contracts;

public interface IPaymentInformationService
{
	[Get("/payments/restaurant/{restaurantId}/payment-information")]
	Task<PaymentInformation> GetPaymentInformation([Path] int restaurantId);
}
