using DemoApp.Services.Customers.BusinessLogic;
using SmingCode.Utilities.AppConfiguration.Config;
using SmingCode.Utilities.Kafka.Host;
using SmingCode.Utilities.Logging.Worker;
using SmingCode.Utilities.ProcessTracking.Config;
using SmingCode.Utilities.ProcessTracking.Kafka.Config;
using SmingCode.Utilities.ServiceMetadata.Config;
using SmingCode.Utilities.StartupProcesses;

KafkaApplicationBuilder builder = KafkaHost.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

if (!builder.Environment.IsDevelopment())
{
    builder.InitializeLogging();
}

configuration.ConnectToAppConfiguration();

services.InitializeServiceMetadata();
services.InitialiseBusinessLogic(configuration);
services.LoadConsumers();

services.AddProcessTracking(tracking =>
    tracking.AddKafkaMiddleware()
);

var host = builder.Build();

await host.RunUserDefinedStartupProcesses();

host.Run();
