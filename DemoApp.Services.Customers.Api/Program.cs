using DemoApp.Services.Customers.BusinessLogic;
using SmingCode.Utilities.Logging.AspNetCore;
using SmingCode.Utilities.MinimalApi;
using SmingCode.Utilities.ProcessTracking;
using SmingCode.Utilities.ProcessTracking.WebApi;
using SmingCode.Utilities.ServiceMetadata;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();

services.InitializeServiceMetadata();

builder.InitializeLogging();

services.InitialiseBusinessLogic(builder.Configuration);

services.AddProcessTracking(tracking =>
    tracking.AddApiMiddleware()
);

var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseProcessTrackingMiddleware();
// app.UseHttpsRedirection();

app.Run();
