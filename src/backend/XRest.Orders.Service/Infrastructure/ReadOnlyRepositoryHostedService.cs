namespace XRest.Orders.Service.Infrastructure;

using XRest.Orders.App.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ReadOnlyRepositoryHostedService : IHostedService, IDisposable
{
	private readonly ILogger<ReadOnlyRepositoryHostedService> _logger;
	private readonly IReadOnlyProductRepository _readOnlyProductRepository;
	private CancellationTokenSource? _cancellationTokenSource;
	private Task _handle;

	public ReadOnlyRepositoryHostedService(IReadOnlyProductRepository readOnlyProductRepository,
		ILogger<ReadOnlyRepositoryHostedService> logger)
	{
		_readOnlyProductRepository = readOnlyProductRepository; 
		_logger = logger;

	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("ReadOnlyRepositoryHostedService -> start");
		_cancellationTokenSource = new CancellationTokenSource();
		_handle = Task.Run(async () => await CheckForNewDataVersion(cancellationToken), _cancellationTokenSource.Token);

		return Task.CompletedTask;
	}

	private async Task CheckForNewDataVersion(CancellationToken cancellationToken)
	{
		TimeSpan timeSpan = new(0, 5, 0);

		while (true)
		{
			_readOnlyProductRepository.LoadAllProducts();

			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			await Task.Delay(timeSpan, cancellationToken);
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("ReadOnlyRepositoryHostedService -> koniec");
		Disable();
		Dispose();
		return Task.CompletedTask;
	}

	private void Disable()
	{
		try
		{
			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
				_handle.Wait();
			}
		}
		catch { }
	}

	public void Dispose()
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}

		GC.SuppressFinalize(this);
	}
}
