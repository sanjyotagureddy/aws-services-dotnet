// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
  Id = Guid.NewGuid(),
  FullName = "John Doe",
  Email = "sanjyot@sanjyot.com",
  GithubUsername = "sanjyotagureddy",
  DateOfBirth = new DateTime(1994, 1, 1)
};


var queueUrl = await sqsClient.GetQueueUrlAsync("customers");
var sendMessageRequest = new SendMessageRequest()
{
  QueueUrl = queueUrl.QueueUrl,
  MessageBody = JsonSerializer.Serialize(customer),
  MessageAttributes = new Dictionary<string, MessageAttributeValue>()
  {
    {
      "MessageType", new MessageAttributeValue()
      {
        DataType = "String",
        StringValue = nameof(CustomerCreated)
      }
    }
  }
};

await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine("Message sent to SQS");