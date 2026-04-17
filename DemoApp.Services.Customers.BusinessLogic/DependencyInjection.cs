using Microsoft.Extensions.Configuration;

namespace DemoApp.Services.Customers.BusinessLogic;
using Dependencies;
using SmingCode.Utilities.Kafka;

public static class DependencyInjection
{
    public static IServiceCollection InitialiseBusinessLogic(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.InitialiseDependencies(configuration);

        return services;
    }
}