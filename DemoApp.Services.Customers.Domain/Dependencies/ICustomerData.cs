namespace DemoApp.Services.Customers.Domain.Dependencies;
using Dtos;

public interface ICustomerData
{
    Task<Guid> CreateCustomer(
        CustomerDto customerDto
    );
    Task<CustomerDto[]> GetAllCustomers();
    Task<CustomerDto> GetCustomerByIdentifier(
        Guid customerIdentifier
    );
}