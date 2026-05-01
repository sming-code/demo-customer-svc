using DemoApp.Services.Customers.BusinessLogic;
using SmingCode.Utilities.Kafka.Config;
using SmingCode.Utilities.Logging.AspNetCore;
using SmingCode.Utilities.ProcessTracking;
using SmingCode.Utilities.ProcessTracking.Kafka.Config;
using SmingCode.Utilities.ProcessTracking.WebApi.Config;
using SmingCode.Utilities.ServiceMetadata.Config;
using SmingCode.Utilities.StartupProcesses;
using SmingCode.Utilities.StartupProcesses.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();

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

app.Run();
