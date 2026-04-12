using Microsoft.EntityFrameworkCore;
using SmingCode.Utilities.StartupProcesses.AspNetCore;

namespace DemoApp.Services.Customers.Dependencies;
using Databases.Customers.Context;

internal class DatabaseInitialization : IServiceInitializer
{
    public Delegate ServiceInitializer =>
        (CustomerContext customerContext) =>
        {
            customerContext.Database.Migrate();
        };
}