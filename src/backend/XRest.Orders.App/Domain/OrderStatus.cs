namespace XRest.Orders.App.Domain;

public enum OrderStatus
{
	/// <summary>
	/// Utworzono
	/// </summary>
	Created = 0,

	/// <summary>
	/// Zaakceptowane
	/// </summary>
	Accepted = 1,

	/// <summary>
	/// Anulowane
	/// </summary>
	Cancel = 2,

	/// <summary>
	/// Odrzucone
	/// </summary>
	Reject = 3,

	/// <summary>
	/// W doręczeniu
	/// </summary>
	InDelivery = 4,

	/// <summary>
	/// Dostarczone
	/// </summary>
	Delivered = 5,

	/// <summary>
	/// Nie dostarczony
	/// </summary>
	NotDelivered = 7,

	/// <summary>
	/// W oczekiwaniu na płatność
	/// </summary>
	PaymentWaiting = 9,

	/// <summary>
	/// Rozpatrzone
	/// </summary>
	Considered = 10,

	//Schowek
	PostPoned = 11,

	/// <summary>
	/// Oczekiwanie na przetworzenie
	/// </summary>
	WaitingForRealization = 99,

	/// <summary>
	/// Zatwierdzono
	/// </summary>
	Approved = 100,

	/// <summary>
	/// W realizacji
	/// </summary>
	InRealization = 101

}