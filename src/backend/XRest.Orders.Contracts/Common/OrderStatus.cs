namespace XRest.Orders.Contracts.Common;

public enum OrderStatus
{
	Created = 0,
	Accepted = 1,
	Cancel = 2,
	Reject = 3,
	InDelivery = 4,
	Delivered = 5,
	NotDelivered = 7,
	PaymentWaiting = 9,
	Considered = 10,
	PostPoned = 11
}
