namespace XRest.Orders.App.Domain;

public class ChangePaymentHistory
{
	public DateTime Date { get; set; }

	public int FromPaymentId { get; set; }

	public int ToPaymentId { get; set; }

	public int WorkerId { get; set; }

	public int BillNumber { get; set; }
}
