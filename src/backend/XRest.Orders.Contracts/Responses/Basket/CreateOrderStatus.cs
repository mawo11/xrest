namespace XRest.Orders.Contracts.Responses.Basket
{
	public enum CreateOrderStatus
	{
		Ok,
		GoToOnlinePayment,
		OnlinePaymentError,
		InvalidNip,
		InvalidOrder,
		InvalidAddress,
		InvalidPhone,
		UnknownError
	}
}
