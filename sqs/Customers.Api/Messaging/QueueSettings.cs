namespace Customers.Api.Messaging;

public class QueueSettings
{
  public const string SectionName = "Queue";
  public required string Name { get; init; }
}