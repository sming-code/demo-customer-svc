using Microsoft.EntityFrameworkCore;

namespace DemoApp.Services.Customers.Dependencies.Databases.Customers;
using Context;
using Context.Models;
using Mappers;

internal class CustomerData(
    CustomerContext _customerContext
) : ICustomerData
{
    public async Task<Guid> CreateCustomer(
        CustomerDto customerDto
    )
    {
        var newEntity = new Customer
        {
            CustomerId = customerDto.CustomerIdentifier,
            FirstName = customerDto.FirstName,
            Surname = customerDto.Surname
        };

        _customerContext.Add(newEntity);

        await _customerContext.SaveChangesAsync();
        return newEntity.CustomerId;
    }

    public async Task<CustomerDto[]> GetAllCustomers()
        => await _customerContext.Customers
            .AsNoTracking()
            .Select(entity => entity.ToDto())
            .ToArrayAsync();

    public async Task<CustomerDto> GetCustomerByIdentifier(
        Guid customerIdentifier
    )
    {
        var customerEntity = await _customerContext
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                entity => entity.CustomerId == customerIdentifier
            )
            ?? throw new Exception("Not good");

        return customerEntity.ToDto();
    }
}