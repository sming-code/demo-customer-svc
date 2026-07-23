using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmingCode.Utilities.ServiceApiClient.Config;
using SmingCode.Utilities.StartupProcesses;

namespace DemoApp.Services.Customers.Dependencies;
using Apis.ReservationService;
using Databases.Customers;
using Databases.Customers.Context;

public static class DependencyInjection
{
    public static IServiceCollection InitialiseDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ICustomerData, CustomerData>();

        var connectionString = configuration["Database:ConnectionString"]
            ?? throw new InvalidOperationException(
                "Attempt to connect to service database requires configuration entry Database:ConnectionString"
            );

        Console.WriteLine(connectionString);

        services.AddDbContext<CustomerContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddApiClient<ReservationServiceApi>(
            "Reservation Service",
            "reservation-svc",
            httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["Apis:ReservationService:BaseAddress"]!);
            }
        );

        services.AddApiClient<ReservationServiceApi>(
            "PPL Service",
            "ppl-svc",
            httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["Apis:ReservationService:BaseAddress"]!);
            }
        );

        services.AddApiClient<ReservationServiceApi>(
            "Broken Service",
            "broken-svc",
            httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["Apis:ReservationService:BaseAddress"]!);
            }
        );

        services.AddScoped<IServiceInitializer, DatabaseInitialization>();

        return services;
    }
}