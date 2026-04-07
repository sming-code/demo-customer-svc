using DemoApp.Services.Customers.Domain.Dtos;

namespace DemoApp.Services.Customers.BusinessLogic;

internal class CustomerService(
    ICustomerApi _customerApi,
    ICustomerData _customerData
) : ICustomerService
{
    public async Task<Guid> CreateCustomer(
        string firstName,
        string surname
    ) => await _customerData.CreateCustomer(
        firstName,
        surname
    );
    public async Task<CustomerDto[]> GetAllCustomers()
        => await _customerApi.GetAll();

    public async Task<CustomerDto> GetCustomerByIdentifier(
        Guid customerIdentifier
    ) => await _customerData.GetCustomerByIdentifier(
        customerIdentifier
    );
}