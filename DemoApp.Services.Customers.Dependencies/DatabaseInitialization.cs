using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmingCode.Utilities.StartupProcesses;

namespace DemoApp.Services.Customers.Dependencies;
using Databases.Customers.Context;

internal class DatabaseInitialization : IServiceInitializer
{
    public Delegate ServiceInitializer =>
        (
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

            customerContext.Database.Migrate();
        };
}