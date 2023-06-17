using XRest.Restaurants.App.Services;
using XRest.Shared.Extensions;
using MediatR;

namespace XRest.Restaurants.App.Queries.Payment.GetPaymentsForOrdering;

public class GetPaymentsForOrderingQueryHandler : IRequestHandler<GetPaymentsForOrderingQuery, Contracts.Payment[]>
{
	private readonly IPaymentRepository _paymentRepository;
	private readonly IRestaurantCache _restaurantFactory;

	public GetPaymentsForOrderingQueryHandler(IPaymentRepository paymentRepository, IRestaurantCache restaurantFactory)
	{
		_paymentRepository = paymentRepository;
		_restaurantFactory = restaurantFactory;
	}

	public async Task<Contracts.Payment[]> Handle(GetPaymentsForOrderingQuery request, CancellationToken cancellationToken)
	{
		var payments = await _paymentRepository.GetAllPaymentsAsync();
		var restaurant = await _restaurantFactory.GetByIdAsync(request.RestaurantId);

		if (restaurant == null)
		{
			return Array.Empty<Contracts.Payment>();
		}

		if (string.IsNullOrEmpty(restaurant.PaymentId))
		{
			return payments
				.Where(static x => x.Ordering && x.Id != 1)
				.Select(x => Map(x, request.Lang))
				.ToArray();
		}

		return payments
		  .Where(static x => x.Ordering)
		  .Select(x => Map(x, request.Lang))
		  .ToArray();
	}

	private static Contracts.Payment Map(Domain.Payment source, string lang)
	{
		return new Contracts.Payment
		{
			Id = source.Id,
			Name = source.NameTranslations.GetTranslate(lang, source.Name)
		};
	}
}
