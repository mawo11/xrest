
using XRest.Orders.App.Commands.Basket.RemoveItem;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Responses.Basket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace XRest.Orders.App.Commands.Basket.UpdateItemNote;
public sealed class UpdateItemNoteCommandHandler : IRequestHandler<UpdateItemNoteCommand, BasketView>
{
	private readonly static BasketView Empty = new()
	{
		CanSubmit = false,
		Empty = true
	};

	private readonly IBasketStorage _basketStorage;
	private readonly ILogger<RemoveItemCommandHandler> _logger;
	private readonly IFingerprintGenerator _fingerprintGenerator;
	private readonly IBasketViewGenerator _basketViewGenerator;

	public UpdateItemNoteCommandHandler(IBasketStorage basketStorage,
		ILogger<RemoveItemCommandHandler> logger,
		IFingerprintGenerator fingerprintGenerator,
		IBasketViewGenerator basketViewGenerator)
	{
		_basketViewGenerator = basketViewGenerator;
		_basketStorage = basketStorage;
		_logger = logger;
		_fingerprintGenerator = fingerprintGenerator;
	}

	public async Task<BasketView> Handle(UpdateItemNoteCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("UpdateItemNotCommandHandler -> basketKey: {BasketKey} ItemId: {ItemId} ",
			request.BasketKey,
			request.ItemId);
		if (string.IsNullOrEmpty(request.BasketKey))
		{
			return Empty;
		}

		var basketData = _basketStorage.GetByKey(request.BasketKey);
		if (basketData == null || basketData.IsLocked)
		{
			return Empty;
		}

		var item = basketData.Items.FirstOrDefault(x => x.Id == request.ItemId);
		if (item != null)
		{
			item.SelectedProducts.Note = request.Note;
			string newId = _fingerprintGenerator.Generate(request.BasketKey, item.SelectedProducts);
			item.AssignNewId(newId);

		}

		return await _basketViewGenerator.GenerateAsync(basketData, request.Lang);
	}
}
