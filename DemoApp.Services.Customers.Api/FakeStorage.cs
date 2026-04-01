using DemoApp.Services.Customers.Api.Models;

namespace DemoApp.Services.Customers.Api;

internal static class FakeStorage
{
    private static readonly Dictionary<Guid, CustomerModel> _customersStorage = [];

    internal static Guid AddCustomer(
        string firstName,
        string surname
    )
    {
        var customerId = Guid.NewGuid();

        _customersStorage.Add(
            customerId,
            new(
                customerId,
                firstName,
                surname
            )
        );

        return customerId;
    }

    internal static IEnumerable<CustomerModel> GetAll()
        => _customersStorage.Values;
    
    internal static CustomerModel GetById(
        Guid customerId
    ) => _customersStorage[customerId];
}