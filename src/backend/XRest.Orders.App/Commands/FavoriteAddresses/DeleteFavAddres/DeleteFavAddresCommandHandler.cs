
using XRest.Orders.App.Services;
using XRest.Orders.Contracts.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace XRest.Orders.App.Commands.FavoriteAddresses.DeleteFavAddres;
public sealed class DeleteFavAddresCommandHandler : IRequestHandler<DeleteFavAddresCommand, ApiOperationResult>
{
	private readonly static ApiOperationResult Error = new(ApiIOperationStatus.Error);
	private readonly static ApiOperationResult Ok = new(ApiIOperationStatus.Ok);

	private readonly IFavoriteAddressRepository _favoriteAddressRepository;
	private readonly ILogger<DeleteFavAddresCommandHandler> _logger;

	public DeleteFavAddresCommandHandler(IFavoriteAddressRepository favoriteAddressRepository, ILogger<DeleteFavAddresCommandHandler> logger)
	{
		_favoriteAddressRepository = favoriteAddressRepository;
		_logger = logger;
	}

	public async Task<ApiOperationResult> Handle(DeleteFavAddresCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("DeleteFavAddresCommandHandler -> {data}" , JsonSerializer.Serialize(request));
		try
		{
			await _favoriteAddressRepository.RemoveAsync(request.AccountId, request.Id);
			return Ok;
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "DeleteFavAddresCommandHandler");
		}

		return Error;
	}
}
