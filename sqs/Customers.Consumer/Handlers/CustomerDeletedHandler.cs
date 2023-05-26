using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerDeletedHandler: IRequestHandler<CustomerDeleted>
{
  private readonly ILogger<CustomerDeletedHandler> _logger;

  public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(CustomerDeleted request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Customer deleted: {Id}", request.Id);
    return;
  }
}