using System.Reflection;
using Amazon.SQS;
using MediatR;
using Customers.Consumer;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.SectionName));
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.Run();
