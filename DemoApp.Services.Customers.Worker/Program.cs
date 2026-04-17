using SmingCode.Utilities.ServiceMetadata;
using SmingCode.Utilities.StartupProcesses;
using DemoApp.Services.Customers.BusinessLogic;

KafkaApplicationBuilder builder = KafkaHost.CreateApplicationBuilder(args);
var services = builder.Services;

services.InitializeServiceMetadata();

services.InitialiseBusinessLogic(builder.Configuration);

services.LoadConsumers();

Console.WriteLine($"{builder.Configuration["Kafka:BootstrapServers"]}");

var host = builder.Build();

await host.RunUserDefinedStartupProcesses();
host.Run();
