using XRest.Orders.App.Domain;
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace XRest.Orders.App.Commands.FavoriteAddresses.SaveFavAddres;
public sealed class SaveFavAddresCommandHandler : IRequestHandler<SaveFavAddresCommand, ApiOperationResult>
{
	private readonly static ApiOperationResult Error = new(ApiIOperationStatus.Error);
	private readonly static ApiOperationResult Ok = new(ApiIOperationStatus.Ok);

	private readonly IFavoriteAddressRepository _favoriteAddressRepository;
	private readonly ILogger<SaveFavAddresCommandHandler> _logger;

	public SaveFavAddresCommandHandler(IFavoriteAddressRepository favoriteAddressRepository, ILogger<SaveFavAddresCommandHandler> logger)
	{
		_favoriteAddressRepository = favoriteAddressRepository;
		_logger = logger;
	}

	public async Task<ApiOperationResult> Handle(SaveFavAddresCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("SetDefaultCommandHandler -> {data}", JsonSerializer.Serialize(request));
		try
		{
			var item = new FavoriteAddressItem
			{
				AccountId = request.AccountId,
				AddressCity = request.Item.AddressCity,
				AddressHouseNumber = request.Item.AddressHouseNumber,
				AddressStreet = request.Item.AddressStreet,
				AddressStreetNumber = request.Item.AddressStreetNumber,
				Default = request.Item.Default,
				Id = request.Item.Id
			};

			await _favoriteAddressRepository.SaveAsync(item);
			return Ok;
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "SetDefaultCommandHandler");
		}

		return Error;
	}
}
