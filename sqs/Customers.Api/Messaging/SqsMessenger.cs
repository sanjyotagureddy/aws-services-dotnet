using System.Text.Json;

using Amazon.SQS;
using Amazon.SQS.Model;

using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
  private readonly IAmazonSQS _sqsClient;
  private readonly IOptions<QueueSettings> _queueSettings;
  private string? _queueUrl;
  public SqsMessenger(IAmazonSQS sqsClient, IOptions<QueueSettings> queueSettings)
  {
    _sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
    _queueSettings = queueSettings ?? throw new ArgumentNullException(nameof(queueSettings));
  }

  public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
  {
    var queueUrl = await GetQueueUrlAsync();
    var sendMessageRequest = new SendMessageRequest
    {
      QueueUrl = queueUrl,
      MessageBody = JsonSerializer.Serialize(message),
      MessageAttributes = new Dictionary<string, MessageAttributeValue>
      {
        {
          "MessageType", new MessageAttributeValue
          {
            DataType = "String",
            StringValue = typeof(T).Name
          }
        }
      }
    };

    return await _sqsClient.SendMessageAsync(sendMessageRequest);
  }

  private async Task<string> GetQueueUrlAsync()
  {
    if (_queueUrl is not null)
    {
      return _queueUrl;
    }
    var queueUrl = await _sqsClient.GetQueueUrlAsync(_queueSettings.Value.Name);
    _queueUrl = queueUrl.QueueUrl;
    return _queueUrl;
  }
}