using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmingCode.Utilities.StartupProcesses.AspNetCore;

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
            logger.LogInformation(
                "Got inside database initialization. db connection string is {connString}",
                customerContext.Database.GetConnectionString()
            );

            File.WriteAllText("/data/test-write.txt", "Hello there");

            customerContext.Database.Migrate();
        };
}