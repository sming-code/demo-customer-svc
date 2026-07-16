namespace DemoApp.Services.Customers.Domain.Dependencies;
using Dtos;

public interface ICustomerData
{
    Task<Guid> CreateCustomer(
        Guid customerIdentifier,
        string firstName,
        string surname
    );
    Task<CustomerDto[]> GetAllCustomers();
    Task<CustomerDto> GetCustomerByIdentifier(
        Guid customerIdentifier
    );
}