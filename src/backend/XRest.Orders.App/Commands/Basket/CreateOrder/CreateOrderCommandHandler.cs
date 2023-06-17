using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;
using XRest.Orders.App.Domain;
using XRest.Orders.Contracts.Request.Basket;
using XRest.Orders.App.Validators;
using XRest.Restaurants.Contracts;
using Microsoft.Extensions.Configuration;
using XRest.Shared.Extensions;
using XRest.Shared.Services;

namespace XRest.Orders.App.Commands.Basket.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
	private readonly static CreateOrderResponse InvalidOrder = new(CreateOrderStatus.InvalidOrder, null, 0);
	private readonly static CreateOrderResponse UnknownError = new(CreateOrderStatus.UnknownError, null, 0);

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<CreateOrderCommandHandler> _logger;
	private readonly IEnumerable<IOrderCreatorValidator> _validators;
	private readonly IOnlineOrderRepository _onlineOrderRepository;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IRestaurantService _resturantService;
	private readonly IPublisher _publisher;
	private readonly IOrderTotalCalculateService _orderTotalCalculateService;
	private readonly IPaymentService _paymentService;
	private readonly IConfiguration _configuration;

	public CreateOrderCommandHandler(IBasketStorage basketStorage,
		ILogger<CreateOrderCommandHandler> logger,
		IEnumerable<IOrderCreatorValidator> validators,
		IOnlineOrderRepository onlineOrderRepository,
		IDateTimeProvider dateTimeProvider,
		IRestaurantService resturantService,
		IPublisher publisher,
		IOrderTotalCalculateService orderTotalCalculateService,
		IPaymentService paymentService,
		IConfiguration configuration)
	{
		_configuration = configuration;
		_paymentService = paymentService;
		_orderTotalCalculateService = orderTotalCalculateService;
		_publisher = publisher;
		_resturantService = resturantService;
		_dateTimeProvider = dateTimeProvider;
		_onlineOrderRepository = onlineOrderRepository;
		_basketStorage = basketStorage;
		_logger = logger;
		_validators = validators;
	}

	public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("CreateOrderCommandHandler -> basketKey: {BasketKey} data: {data}",
			request.BasketKey,
			CustomJsonSerializer.Serialize(request));
		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return InvalidOrder;
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null)
		{
			return InvalidOrder;
		}

		if (!basketData.TryLock())
		{
			return InvalidOrder;
		}

		foreach (var validator in _validators)
		{
			//TODO: inne walidator: telefon, email, 
			var status = await validator.ValidateAsync(basketData, request);
			if (status != OrderCreatorValidatorStatus.Ok)
			{
				_logger.LogError("CreateOrderCommandHandler -> błedy walidacji {status} dla request: {data}",
					status,
					System.Text.Json.JsonSerializer.Serialize(request));
				basketData.Unlock();
				return CreateOrderResponse(status);
			}
		}

		var restInfo = await _resturantService.GetInformationForOrderingAsync(basketData.RestaurantId);
		if (restInfo == null)
		{
			basketData.Unlock();
			_logger.LogError("Nie można pobrać informacji o restauracji: {RestaurantId} ", basketData.RestaurantId);
			return InvalidOrder;
		}

		NewOnlineOrder newOnlineOrder = new()
		{
			Created = _dateTimeProvider.Now,
			Modified = _dateTimeProvider.Now,
			AuditDate = _dateTimeProvider.Now,
			DeliveryType = basketData.Type,
			Source = basketData.Source,
			Email = request.Request.Email,
			Note = request.Request.Note,
			Phone = request.Request.Phone,
			Discount = basketData.DiscountValue,
			OrderAddress = Map(request.Request.Delivery),
			PaymentId = request.Request.PaymentId,
			TermType = TermType.Now,
			RestaurantId = basketData.RestaurantId,
			OrderDay = restInfo.OrderDay,
			PayedOnline = false,
			TransportZoneId = basketData.TransportZoneId,
			Items = basketData.Items,
			Firstname = request.Request.Firstname,
			MarketingIds = request.Request.MarketingIds ?? Array.Empty<int>(),
			Marketing = request.Request.MarketingIds != null && request.Request.MarketingIds.Length > 0,
			Realization = true,
			TermOfUse = request.Request.TermOfUse,
			Amount = await _orderTotalCalculateService.CalculateAsync(basketData),
			Status = request.Request.PaymentId == 1 ? OrderStatus.PaymentWaiting : OrderStatus.Created,
			OnlineInvoice = Map(request.Request.Invoice),
			TransportAmount = basketData.DeliveryPrice,
			BagProduct = basketData.BagProduct,
			DiscountCode = basketData.DiscountCode,
			GratisProduct = basketData.GratisProduct
		};

		if (request.AccountId > 0)
		{
			newOnlineOrder.CustomerId = request.AccountId;
		}

		int orderId = await _onlineOrderRepository.SaveAsync(newOnlineOrder);

		if (orderId != -1)
		{
			_logger.LogInformation("Zamowienie utworzone: {orderId}", orderId);

			await _publisher.Publish(new NewOnlineOrderCreatedNotification(orderId, newOnlineOrder), cancellationToken);

			_basketStorage.Remove(request.BasketKey);

			if (request.Request.PaymentId == 1)
			{
				var registerPaymentResult = await _paymentService.RegisterPaymentAsync(orderId, newOnlineOrder);
				if (!registerPaymentResult.Success)
				{
					PaymentInfo paymentInfo = new()
					{
						Created = _dateTimeProvider.Now,
						Response = registerPaymentResult.Response,
						Type = "przelewy24"
					};

					await _onlineOrderRepository.UpdatePaymentStatusAsync(orderId, OrderStatus.PaymentWaiting, true, CustomJsonSerializer.Serialize(paymentInfo), _dateTimeProvider.Now);

					return new(CreateOrderStatus.OnlinePaymentError, null, orderId);
				}

				string paymentUrl = _configuration.GetValue("Przelewy24:Przelewy24Url", string.Empty);
				string linkToPayments = $"{paymentUrl}trnRequest/{registerPaymentResult.Token}";

				return new(CreateOrderStatus.GoToOnlinePayment, linkToPayments, orderId);
			}

			return new(CreateOrderStatus.Ok, null, orderId);
		}

		basketData.Unlock();
		return UnknownError;
	}

	private static OnlineOrderAddress? Map(DeliveryAddress? delivery)
	{
		if (delivery != null)
		{
			return new OnlineOrderAddress
			{
				City = delivery.City,
				HouseNumber = delivery.HouseNumber,
				Street = delivery.Street,
				StreetNumber = delivery.StreetNumber
			};
		}

		return null;
	}

	private static OnlineInvoice? Map(InvoiceData? invoice)
	{
		if (invoice != null)
		{
			return new OnlineInvoice()
			{
				Address = invoice.Address,
				Name = invoice.Name,
				Nip = invoice.Nip
			};
		}

		return null;
	}

	private static CreateOrderResponse CreateOrderResponse(OrderCreatorValidatorStatus status)
	{
		CreateOrderStatus createOrderStatus = CreateOrderStatus.UnknownError;
		switch (status)
		{
			case OrderCreatorValidatorStatus.Ok:
				createOrderStatus = CreateOrderStatus.Ok;
				break;
			case OrderCreatorValidatorStatus.InvalidNip:
				createOrderStatus = CreateOrderStatus.InvalidNip;
				break;
		}

		return new CreateOrderResponse(createOrderStatus, null, 0);
	}
}
