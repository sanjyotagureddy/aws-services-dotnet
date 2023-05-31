using System.Text.Json;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using Customers.Api.Contracts.Messages;

using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SnsMessenger : ISnsMessenger
{
  private readonly IAmazonSimpleNotificationService _sns;
  private readonly IOptions<TopicSettings> _topicSettings;
  private string? _topicArn;

  public SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings)
  {
    _sns = sns;
    _topicSettings = topicSettings;
  }


  private async ValueTask<string> GetTopicArnAsync()
  {
    if (_topicArn is not null)
    {
      return _topicArn;
    }

    var queueUrlResponse = await _sns.FindTopicAsync(_topicSettings.Value.Name);
    _topicArn = queueUrlResponse.TopicArn;
    return _topicArn;
  }

  public async Task<PublishResponse> PublishMessageAsync<T>(T message)
  {
    var topicArn = await GetTopicArnAsync();
    var publishRequest = new PublishRequest()
    {
      TopicArn = topicArn,
      Message = JsonSerializer.Serialize(message),
      MessageAttributes = new Dictionary<string, MessageAttributeValue>()
        {
          {
            "MessageType", new MessageAttributeValue()
            {
              DataType = "String",
              StringValue = typeof(T).Name
            }
          }
        }
    };

    return await _sns.PublishAsync(publishRequest);
  }
}
