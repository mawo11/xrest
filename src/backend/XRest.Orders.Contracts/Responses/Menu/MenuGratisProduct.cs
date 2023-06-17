
using XRest.Orders.Contracts.Responses.Basket;

namespace XRest.Orders.Contracts.Responses.Menu;
public class MenuGratisProduct
{
	public int Id { get; set; }

	public string? Name { get; set; }

	public int Points { get; set; }

	public ProductSet[]? ProductSets { get; set; }
}
