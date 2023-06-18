using XRest.Identity.App.Domain;
using XRest.Identity.App.Services;
using XRest.Identity.Contracts.Common;
using XRest.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using XRest.Shared.Services;

namespace XRest.Identity.App.Commands.Customers.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenData>
{
	private readonly ILogger<RefreshTokenCommandHandler> _logger;
	private readonly ITokenRepository _tokenRepository;
	private readonly IUserTokenGenerator _userTokenGenerator;
	private readonly IDateTimeProvider _dateTimeProvider;

	public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger,
		ITokenRepository tokenRepository,
		IUserTokenGenerator userTokenGenerator,
		IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
		_tokenRepository = tokenRepository;
		_userTokenGenerator = userTokenGenerator;
	}

	public async Task<TokenData> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("RefreshTokenCommandHandler {data}", CustomJsonSerializer.Serialize(request));

		RefreshTokenData tokenData = await _tokenRepository.GetByTokenAsync(request.Token);
		if (tokenData == null)
		{
			return TokenData.InvalidToken;
		}

		if (tokenData.Expired >= _dateTimeProvider.Now)
		{
			return TokenData.InvalidToken;
		}

		await _tokenRepository.RemoveByIdAsync(tokenData.Id);

		return await _userTokenGenerator.GenerateAsync(request.AccountId);
	}
}
