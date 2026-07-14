using DemoApp.Services.Customers.BusinessLogic;
using SmingCode.Utilities.AppConfiguration.Config;
using SmingCode.Utilities.Logging.AspNetCore;
using SmingCode.Utilities.ProcessTracking.Config;
using SmingCode.Utilities.ProcessTracking.Kafka.Config;
using SmingCode.Utilities.ProcessTracking.WebApi.Config;
using SmingCode.Utilities.ServiceMetadata.Config;
using SmingCode.Utilities.ServiceMetadata.WebApplicationStartup;
using SmingCode.Utilities.StartupProcesses;
using SmingCode.Utilities.StartupProcesses.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();

configuration.ConnectToAppConfiguration();

services.InitializeServiceMetadata();
builder.InitializeLogging();

services.InitialiseBusinessLogic(configuration);
services.LoadConsumers();
services.InitializeKafkaHandling(
    configuration,
    true
);

services.AddProcessTracking(tracking =>
    tracking.AddApiMiddleware()
        .AddKafkaMiddleware()
);

var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseProcessTrackingMiddleware();
await app.RunUserDefinedStartupProcesses(
    dependencyManager => dependencyManager.EnableAspNetCore()
);

app.RunWithServiceMetadataLoggerScope();
