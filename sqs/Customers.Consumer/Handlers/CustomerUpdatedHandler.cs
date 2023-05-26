using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
{
  private readonly ILogger<CustomerUpdated> _logger;

  public CustomerUpdatedHandler(ILogger<CustomerUpdated> logger)
  {
    _logger = logger;
  }

  public async Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Customer updated: {Id}", request.FullName);
    return;
  }
}