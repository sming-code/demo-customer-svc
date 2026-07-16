namespace DemoApp.Services.Customers.Domain.Definition.Services;
using Dtos;

public interface ICustomerService
{
    Task<Guid> QueueCreateCustomer(
        string firstName,
        string surname
    );
    Task CreateCustomer(
        Guid customerIdentifier,
        string firstName,
        string surname
    );
    Task<CustomerDto[]> GetAllCustomers();
    Task<CustomerDto> GetCustomerByIdentifier(
        Guid customerIdentifier
    );
}