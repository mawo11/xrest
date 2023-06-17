using XRest.Restaurants.App.Services;

namespace XRest.Restaurants.Service.Infrastructure;

public class TransportZoneCacheCheckerService : IHostedService, IDisposable
{
	private readonly ITransportZoneFinderService _transportZoneFinderService;
	private readonly ITransportZonesRepository _transportZonesRepository;
	private readonly ILogger<LocationCacheCheckerService> _logger;
	private DateTime _currentDate;
	private CancellationTokenSource? _cancellationTokenSource;

	public TransportZoneCacheCheckerService(ITransportZoneFinderService transportZoneFinderService,
		ITransportZonesRepository transportZonesRepository,
		ILogger<LocationCacheCheckerService> logger,
		IHostApplicationLifetime hostApplicationLifetime)
	{
		_transportZoneFinderService = transportZoneFinderService;
		_transportZonesRepository = transportZonesRepository;
		_logger = logger;

		hostApplicationLifetime.ApplicationStopping.Register(Disable);
	}

	~TransportZoneCacheCheckerService()
	{
		Dispose(false);
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("TransportZoneCacheCheckerService -> start");

		_cancellationTokenSource = new CancellationTokenSource();
		_ = Task.Run(async () => await CheckForNewDataVersion(cancellationToken), _cancellationTokenSource.Token);

		return Task.CompletedTask;
	}

	private async Task CheckForNewDataVersion(CancellationToken cancellationToken)
	{
		TimeSpan timeSpan = new(0, 1, 0);

		var items = await _transportZonesRepository.LoadAllAsync();
		if (items.Any())
		{
			_currentDate = items.Max(x => x.AuditDate);
			_logger.LogInformation("TransportZoneCacheCheckerService -> _transportZoneFinderService.LoadData -> start");
			_transportZoneFinderService.LoadData(items);
			_logger.LogInformation("TransportZoneCacheCheckerService -> _transportZoneFinderService.LoadData -> end");
		}

		while (true)
		{
			await Task.Delay(timeSpan, cancellationToken);
			if (!await _transportZonesRepository.IsNewDataAvailableAsync(_currentDate))
			{
				continue;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			items = await _transportZonesRepository.LoadAllAsync();
			if (items.Any())
			{
				_currentDate = items.Max(x => x.AuditDate);
				_transportZoneFinderService.LoadData(items);
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("TransportZoneCacheCheckerService -> koniec");
		Disable();
		return Task.CompletedTask;
	}

	private void Disable()
	{
		try
		{
			if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
			{
				_cancellationTokenSource.Cancel();
			}
		}
		catch { }
	}

	public void Dispose()
	{
		Dispose(true);
	}

	private void Dispose(bool disposing)
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}

		if (!disposing)
		{
			GC.SuppressFinalize(this);
		}
	}
}
