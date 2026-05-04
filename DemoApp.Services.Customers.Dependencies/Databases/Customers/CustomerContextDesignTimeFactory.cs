using Microsoft.EntityFrameworkCore.Design;

namespace DemoApp.Services.Customers.Dependencies.Databases.Customers;
using Context;
using Microsoft.EntityFrameworkCore;

public class CustomerContextFactory : IDesignTimeDbContextFactory<CustomerContext>
{
    public CustomerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>()
            .UseSqlServer("Data Source=/home/matt/data/customer.db");

        return new CustomerContext(optionsBuilder.Options);
    }
}