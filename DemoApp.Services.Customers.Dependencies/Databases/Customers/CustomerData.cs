using Microsoft.EntityFrameworkCore;

namespace DemoApp.Services.Customers.Dependencies.Databases.Customers;

using System.Text.Json;
using Context;
using Context.Models;
using Mappers;
using Microsoft.Extensions.Logging;

internal class CustomerData(
    CustomerContext _customerContext,
    ILogger<CustomerData> _logger
) : ICustomerData
{
    public async Task<Guid> CreateCustomer(
        CustomerDto customerDto
    )
    {
        await using var transaction = await _customerContext.Database.BeginTransactionAsync();

        var newEntity = new Customer
        {
            CustomerId = customerDto.CustomerIdentifier,
            FirstName = customerDto.FirstName,
            Surname = customerDto.Surname
        };

        _customerContext.Add(newEntity);

        await _customerContext.SaveChangesAsync();

        await transaction.CommitAsync();

        var updatedSet = await _customerContext.Customers
            .ToListAsync();
        var message = JsonSerializer.Serialize(updatedSet);
        _logger.LogInformation(message);

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

        var message = JsonSerializer.Serialize(customerEntity);

        _logger.LogInformation(message);
        return customerEntity.ToDto();
    }
}