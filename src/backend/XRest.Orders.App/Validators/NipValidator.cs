using XRest.Orders.App.Commands.Basket.CreateOrder;
using XRest.Orders.App.Domain;

namespace XRest.Orders.App.Validators;

internal class NipValidator : IOrderCreatorValidator
{
	public ValueTask<OrderCreatorValidatorStatus> ValidateAsync(BasketData data, CreateOrderCommand createOrderCommand)
	{
		if (createOrderCommand.Request.Invoice == null)
		{
			return ValueTask.FromResult(OrderCreatorValidatorStatus.Ok);
		}

		if (string.IsNullOrEmpty(createOrderCommand.Request.Invoice.Nip))
		{
			return ValueTask.FromResult(OrderCreatorValidatorStatus.InvalidNip);
		}

		return ValueTask.FromResult(NIPValidate(createOrderCommand.Request.Invoice.Nip) ?
				OrderCreatorValidatorStatus.Ok :
				OrderCreatorValidatorStatus.InvalidNip);
	}

	static public bool NIPValidate(string NIPValidate)
	{
		const byte length = 10;
		byte[] digits;
		byte[] weights = new byte[] { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

		if (NIPValidate.Length.Equals(length).Equals(false)) return false;
		if (ulong.TryParse(NIPValidate, out _).Equals(false)) return false;
		else
		{
			string sNIP = NIPValidate.ToString();
			digits = new byte[length];

			for (int i = 0; i < length; i++)
			{
				if (byte.TryParse(sNIP[i].ToString(), out digits[i]).Equals(false)) return false;
			}

			int checksum = 0;

			for (int i = 0; i < length - 1; i++)
			{
				checksum += digits[i] * weights[i];
			}

			return (checksum % 11 % 10).Equals(digits[digits.Length - 1]);
		}
	}
}