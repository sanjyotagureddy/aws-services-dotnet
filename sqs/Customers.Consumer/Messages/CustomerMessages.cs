using MediatR;

namespace Customers.Consumer.Messages;

public class CustomerCreated : ISqsMessage
{
  public required Guid Id { get; set; }
  public required string FullName { get; set; }
  public required string Email { get; set; }
  public required string GithubUsername { get; set; }
  public required DateTime DateOfBirth { get; set; }
}

public class CustomerUpdated : ISqsMessage
{
  public required Guid Id { get; set; }
  public required string FullName { get; set; }
  public required string Email { get; set; }
  public required string GithubUsername { get; set; }
  public required DateTime DateOfBirth { get; set; }
}

public class CustomerDeleted : ISqsMessage
{
  public required Guid Id { get; set; }
}