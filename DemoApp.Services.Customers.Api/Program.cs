using Azure.Identity;
using DemoApp.Services.Customers.BusinessLogic;
using SmingCode.Utilities.Logging.AspNetCore;
using SmingCode.Utilities.ProcessTracking.Config;
using SmingCode.Utilities.ProcessTracking.Kafka.Config;
using SmingCode.Utilities.ProcessTracking.WebApi.Config;
using SmingCode.Utilities.ServiceMetadata.Config;
using SmingCode.Utilities.ServiceMetadata.WebApplicationStartup;
using SmingCode.Utilities.StartupProcesses;
using SmingCode.Utilities.StartupProcesses.AspNetCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();

var appConfigurationEndpoint = builder.Configuration.GetValue<string>("App_Config_Endpoint")!;
var appConfigurationLabel = builder.Configuration.GetValue<string>("Tag_Environment")!;
Console.WriteLine(appConfigurationLabel);
builder.Configuration.AddAzureAppConfiguration(azureAppConfigurationOptions =>
    azureAppConfigurationOptions
        .Connect(
            new Uri(appConfigurationEndpoint),
            new DefaultAzureCredential()
        )
        .Select(KeyFilter.Any, LabelFilter.Null)
        .Select(KeyFilter.Any, appConfigurationLabel)
);
services.InitializeServiceMetadata();
builder.InitializeLogging();

services.InitialiseBusinessLogic(builder.Configuration);
services.LoadConsumers();
services.InitializeKafkaHandling(
    builder.Configuration,
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
