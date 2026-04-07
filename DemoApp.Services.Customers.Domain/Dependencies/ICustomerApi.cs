namespace DemoApp.Services.Customers.Domain.Dependencies;
using Dtos;

public interface ICustomerApi
{
    Task<CustomerDto[]> GetAll();
}
