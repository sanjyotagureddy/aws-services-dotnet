using Customers.Api.Contracts;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToMessageMapper
{
  public static CustomerCreated ToCustomerCreated(this Customer customer)
  {
    return new CustomerCreated
    {
      Id = customer.Id,
      FullName = customer.FullName,
      Email = customer.Email,
      GithubUsername = customer.GitHubUsername,
      DateOfBirth = customer.DateOfBirth
    };
  }

  public static CustomerUpdated ToCustomerUpdated(this Customer customer)
  {
    return new CustomerUpdated
    {
      Id = customer.Id,
      FullName = customer.FullName,
      Email = customer.Email,
      GithubUsername = customer.GitHubUsername,
      DateOfBirth = customer.DateOfBirth
    };
  }

  public static CustomerDeleted ToCustomerDeleted(this Customer customer)
  {
    return new CustomerDeleted
    {
      Id = customer.Id
    };
  }
}