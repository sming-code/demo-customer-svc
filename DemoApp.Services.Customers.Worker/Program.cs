using DemoApp.Services.Customers.BusinessLogic;
using SmingCode.Utilities.Kafka.Host;
using SmingCode.Utilities.Logging.Worker;
using SmingCode.Utilities.ProcessTracking.Config;
using SmingCode.Utilities.ProcessTracking.Kafka.Config;
using SmingCode.Utilities.ServiceMetadata.Config;
using SmingCode.Utilities.StartupProcesses;

KafkaApplicationBuilder builder = KafkaHost.CreateApplicationBuilder(args);
var services = builder.Services;

if (!builder.Environment.IsDevelopment())
{
    builder.InitializeLogging();
}

services.InitializeServiceMetadata();
services.InitialiseBusinessLogic(builder.Configuration);
services.LoadConsumers();

services.AddProcessTracking(tracking =>
    tracking.AddKafkaMiddleware()
);

var host = builder.Build();

await host.RunUserDefinedStartupProcesses();

host.Run();
