using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DemoApp.Services.Customers.Dependencies;
using Databases.Customers;
using Databases.Customers.Context;
using DemoApp.Services.Customers.Dependencies.Apis.Customer;

public static class DependencyInjection
{
    public static IServiceCollection InitialiseDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ICustomerData, CustomerData>();

        var customersDbConnString = configuration.GetConnectionString("CustomersDatabase")
            ?? throw new InvalidOperationException(
                "Attempt to connect to customers database requires connection string with name 'CustomersDatabase'"
            );

        services.AddDbContext<CustomerContext>(options =>
        {
            options.UseSqlServer(customersDbConnString);
        });

        services.AddHttpClient<CustomerHttpClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["Apis:Customer:BaseAddress"]!);
            httpClient.Timeout = TimeSpan.FromSeconds(long.Parse(configuration["Apis:Customer:TimeoutSeconds"]!));
            // httpClient.DefaultRequestHeaders.Add(
            //     "Ocp-Apim-Subscription-Key",
            //     configuration["Apis:Customer:SubscriptionKey"]
            // );
        });
        services.AddScoped<ICustomerApi, CustomerApi>();

        return services;
    }
}