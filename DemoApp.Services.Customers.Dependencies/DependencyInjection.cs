using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmingCode.Utilities.StartupProcesses;

namespace DemoApp.Services.Customers.Dependencies;
using Databases.Customers;
using Databases.Customers.Context;
using Microsoft.Data.Sqlite;

public static class DependencyInjection
{
    public static IServiceCollection InitialiseDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ICustomerData, CustomerData>();

        var databaseDirectory = configuration["Database:Directory"];
        var databaseName = configuration["Database:Name"]
            ?? throw new InvalidOperationException(
                "Attempt to connect to service database requires configuration entry Database:Name"
            );

        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = Path.Join(
                databaseDirectory,
                $"{databaseName}.db"
            ),
            Cache = SqliteCacheMode.Shared
        }.ConnectionString;

        Console.WriteLine(connectionString);

        services.AddDbContext<CustomerContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IServiceInitializer, DatabaseInitialization>();

        return services;
    }
}