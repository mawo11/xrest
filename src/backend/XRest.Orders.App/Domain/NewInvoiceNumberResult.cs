namespace XRest.Orders.App.Domain;

public class NewInvoiceNumberResult
{
	public int Number { get; set; }

	public bool Success { get; set; }

	public string? Message { get; set; }


	public static NewInvoiceNumberResult Invalid = new() { Number = -1, Success = false };
}
