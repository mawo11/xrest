
using XRest.Orders.App.Domain;

namespace XRest.Orders.Infrastructure.MsSql.Internal.Online;
internal class OnlineOrderPositionTable
{
	public int Id { get; set; }

	public int ProductId { get; set; }

	public decimal Price { get; set; }

	public ProductType TypeId { get; set; }

	public string? DisplayName { get; set; }

	public byte FromSourceId { get; set; }

	public short Index { get; set; }

	public string? Note { get; set; }

	public short SubIndex { get; set; }

	public decimal BasePrice { get; set; }

	public int Vat { get; set; }

	public int OrderProductId { get; set; }

	public byte Printer { get; set; }

	public int ProductGroupId { get; set; }

	public string? FiscalName { get; set; }

	public string? VatName { get; set; }

	public string? VatGroup { get; set; }

	public bool FiscalPrint { get; set; }

	public int VatId { get; set; }

	public string? Name { get; set; }
}
