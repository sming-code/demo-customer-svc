using SmingCode.Utilities.Kafka;
using SmingCode.Utilities.Kafka.MinimalConsumers;

KafkaApplicationBuilder builder = KafkaHost.CreateApplicationBuilder(args);

builder.LoadConsumers();

Console.WriteLine($"{builder.Configuration["Kafka:BootstrapServers"]}");

var host = builder.Build();

host.Run();
