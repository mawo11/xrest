using MediatR;
using XRest.Identity.Contracts.Customers.Responses;
using XRest.Identity.App.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace XRest.Identity.App.Queries.Customers.GetCustomerPointHistory;

public sealed class GetCustomerPointHistoryQueryHandler : IRequestHandler<GetCustomerPointHistoryQuery, CustomerPointsHistory>
{
	private readonly IAccountRepository _accountRepository;
	private readonly ILogger<GetCustomerPointHistoryQueryHandler> _logger;

	public GetCustomerPointHistoryQueryHandler(IAccountRepository accountRepository, ILogger<GetCustomerPointHistoryQueryHandler> logger)
	{
		_accountRepository = accountRepository;
		_logger = logger;
	}

	public async Task<CustomerPointsHistory> Handle(GetCustomerPointHistoryQuery request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("GetCustomerPointHistoryQueryHandler => {data}", JsonSerializer.Serialize(request));

		var items = await _accountRepository.GetAccountPointsHistoryAsync(request.AccountId);

		List<CustomerPoint> customerPoints = new();
		int total = 0;
		foreach (var item in items)
		{
			total += item.Points;
			customerPoints.Add(new CustomerPoint
			{
				Comment = item.Comment,
				Points = item.Points,
				Created = item.Created.ToString("yyyy-MM-dd HH:mm")
			});
		}

		return new CustomerPointsHistory
		{
			Items = customerPoints,
			Total = total
		};
	}
}
