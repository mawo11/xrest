using XRest.Restaurants.App.Services;
using XRest.Restaurants.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Restaurants.App.Queries.Payment.GetPaymentInformation;

public sealed class GetPaymentInformationQueryHandler : IRequestHandler<GetPaymentInformationQuery, PaymentInformation>
{
	private readonly static PaymentInformation NotExists = new(false, null, null);
	private readonly IRestaurantCache _restaurantFactory;
	private readonly ILogger<GetPaymentInformationQueryHandler> _logger;

	public GetPaymentInformationQueryHandler(IRestaurantCache restaurantFactory, ILogger<GetPaymentInformationQueryHandler> logger)
	{
		_restaurantFactory = restaurantFactory;
		_logger = logger;
	}

	public async Task<PaymentInformation> Handle(GetPaymentInformationQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetRestaurantWorkingStatusQueryHandler => restId: {rest}", request.RestaurantId);
		var rest = await _restaurantFactory.GetByIdAsync(request.RestaurantId);
		if (rest == null)
		{
			return NotExists;
		}

		return new PaymentInformation(true, rest.PaymentId, rest.PaymentSecret);
	}
}
