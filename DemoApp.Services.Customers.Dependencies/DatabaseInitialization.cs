using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmingCode.Utilities.StartupProcesses;

namespace DemoApp.Services.Customers.Dependencies;
using Databases.Customers.Context;

internal class DatabaseInitialization : IServiceInitializer
{
    public Delegate ServiceInitializer =>
        async (
            CustomerContext customerContext,
            ILogger<DatabaseInitialization> logger
        ) =>
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "Got inside database initialization. db connection string is {connString}",
                    customerContext.Database.GetConnectionString()
                );
            }
            
            var pendingMigrations = await customerContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                customerContext.Database.Migrate();

                await customerContext.Database.ExecuteSqlRawAsync(
                    "PRAGMA journal_mode=WAL;"
                );
            }
        };
}