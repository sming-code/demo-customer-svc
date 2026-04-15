using SmingCode.Utilities.Kafka.Host;
using SmingCode.Utilities.ServiceMetadata;
using SmingCode.Utilities.StartupProcesses;
using DemoApp.Services.Customers.BusinessLogic;

KafkaApplicationBuilder builder = KafkaHost.CreateApplicationBuilder(args);
var services = builder.Services;

services.InitializeServiceMetadata();

services.InitialiseBusinessLogic(builder.Configuration);

builder.LoadConsumers();

Console.WriteLine($"{builder.Configuration["Kafka:BootstrapServers"]}");

var host = builder.Build();

await host.RunUserDefinedStartupProcesses();
host.Run();
