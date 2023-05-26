using Amazon.SQS.Model;
using Customers.Api.Contracts.Data;

namespace Customers.Api.Messaging;

public interface ISqsMessenger
{
  Task<SendMessageResponse> SendMessageAsync<T>(T message);
}