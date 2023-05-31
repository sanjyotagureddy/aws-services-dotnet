using System.Text.Json;

using Amazon.SQS;
using Amazon.SQS.Model;

using Customers.Consumer.Messages;

using MediatR;

using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
  private readonly IAmazonSQS _sqs;
  private readonly IOptions<QueueSettings> _queueSettings;
  private readonly IMediator _mediator;
  private readonly ILogger<QueueConsumerService> _logger;

  public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings, IMediator mediator, ILogger<QueueConsumerService> logger)
  {
    _sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
    _queueSettings = queueSettings ?? throw new ArgumentNullException(nameof(queueSettings));
    _mediator = mediator;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    var queueUrl = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);
    var receiveMessageRequest = new ReceiveMessageRequest()
    {
      QueueUrl = queueUrl.QueueUrl,
      AttributeNames = new List<string> { "All" },
      MessageAttributeNames = new List<string> { "All" },
      MaxNumberOfMessages = 1
    };

    while (!stoppingToken.IsCancellationRequested)
    {
      var receiveMessageResponse = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
      foreach (var message in receiveMessageResponse.Messages)
      {
        var messageType = message.MessageAttributes["MessageType"].StringValue;
        _logger.LogInformation($"Message Id: {message.MessageId}");
        _logger.LogInformation($"Message Body: {message.Body}");

        var type = Type.GetType($"Customers.Consumer.Messages.{messageType}");
        if (type is null)
        {
          _logger.LogWarning($"Unknown message type: {messageType}");
        }
        try
        {
          var typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;
          await _mediator.Send(typedMessage, stoppingToken);
        }
        catch (Exception e)
        {
          _logger.LogError(e, "Message failed during processing");
          continue;
        }

        var deleteMessageRequest = new DeleteMessageRequest()
        {
          QueueUrl = queueUrl.QueueUrl,
          ReceiptHandle = message.ReceiptHandle
        };
        await _sqs.DeleteMessageAsync(deleteMessageRequest, stoppingToken);
      }
    }
  }
}