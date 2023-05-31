// See https://aka.ms/new-console-template for more information

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var snsClient = new AmazonSimpleNotificationServiceClient();

var customer = new CustomerCreated
{
  Id = Guid.NewGuid(),
  FullName = "John Doe",
  Email = "sanjyot@sanjyot.com",
  GithubUsername = "sanjyotagureddy",
  DateOfBirth = new DateTime(1994, 1, 1)
};

var topicArnResponse = await snsClient.FindTopicAsync("customers");
var publishRequest = new PublishRequest()
{
  TopicArn = topicArnResponse.TopicArn,
  Message = JsonSerializer.Serialize(customer),
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

var response = snsClient.PublishAsync(publishRequest);