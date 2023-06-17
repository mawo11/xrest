using XRest.Restaurants.App.Services;

namespace XRest.Restaurants.Service.Infrastructure;

public class LocationCacheCheckerService : IHostedService, IDisposable
{
	private readonly ILocationFinderService _locationFinderService;
	private readonly IAddressRepository _addressRepository;
	private readonly ILogger<LocationCacheCheckerService> _logger;
	private CancellationTokenSource? _cancellationTokenSource;
	private DateTime _currentDate;

	public LocationCacheCheckerService(ILocationFinderService locationFinderService,
		IAddressRepository addressRepository,
		ILogger<LocationCacheCheckerService> logger)
	{
		_locationFinderService = locationFinderService;
		_addressRepository = addressRepository;
		_logger = logger;
	}
	~LocationCacheCheckerService()
	{
		Dispose(false);
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("LocationCacheCheckerService -> start");
		_cancellationTokenSource = new CancellationTokenSource();
		_ = Task.Run(async () => await CheckForNewDataVersion(cancellationToken), _cancellationTokenSource.Token);

		return Task.CompletedTask;
	}

	private async Task CheckForNewDataVersion(CancellationToken cancellationToken)
	{
		TimeSpan timeSpan = new(0, 1, 0);

		var items = await _addressRepository.LoadAllAdressesAsync();
		if (items.Any())
		{
			_currentDate = items.Max(x => x.AuditDate);
			_locationFinderService.LoadData(items);
		}

		while (true)
		{
			await Task.Delay(timeSpan, cancellationToken);
			if (!await _addressRepository.IsNewDataAvailableAsync(_currentDate))
			{
				continue;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			items = await _addressRepository.LoadAllAdressesAsync();
			if (items.Any())
			{
				_currentDate = items.Max(x => x.AuditDate);
				_locationFinderService.LoadData(items);
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("LocationCacheCheckerService -> koniec");
		Disable();

		return Task.CompletedTask;
	}

	private void Disable()
	{
		try
		{
			if (_cancellationTokenSource != null)
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
