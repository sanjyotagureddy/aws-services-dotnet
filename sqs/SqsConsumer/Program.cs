// See https://aka.ms/new-console-template for more information

using System.Text.Json;

using Amazon.SQS;
using Amazon.SQS.Model;

using SqsConsumer;

var cts = new CancellationTokenSource();
var sqsClient = new AmazonSQSClient();

var queueUrl = await sqsClient.GetQueueUrlAsync("customers");

var receiveMessageRequest = new ReceiveMessageRequest()
{
  QueueUrl = queueUrl.QueueUrl,
  WaitTimeSeconds = 20,
  AttributeNames = new List<string>()
  {
    "All"
  },
  MessageAttributeNames = new List<string>()
  {
    "All"
  }
};

while (!cts.IsCancellationRequested)
{


  var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
  foreach (var message in receiveMessageResponse.Messages)
  {
    Console.WriteLine($"Message Id: {message.MessageId}");
    Console.WriteLine($"Message Body: {message.Body}");
    //var messageType = message.MessageAttributes["MessageType"].StringValue;
    //switch (messageType)
    //{
    //  case nameof(CustomerCreated):
    //    var customerCreated = JsonSerializer.Deserialize<CustomerCreated>(message.Body);
    //    Console.WriteLine($"Customer created: {customerCreated.FullName}");
    //    break;
    //  default:
    //    Console.WriteLine($"Unknown message type: {messageType}");
    //    break;
    //}
    var deleteMessageRequest = new DeleteMessageRequest()
    {
      QueueUrl = queueUrl.QueueUrl,
      ReceiptHandle = message.ReceiptHandle
    };
    await sqsClient.DeleteMessageAsync(deleteMessageRequest);
  }
  await Task.Delay(3000);
}