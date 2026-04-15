using DemoApp.Services.Customers.Domain.Dtos;
using SmingCode.Utilities.Kafka.Producers;

namespace DemoApp.Services.Customers.BusinessLogic;

internal class CustomerService(
    ICustomerData _customerData,
    IKafkaProducer _kafkaProducer
) : ICustomerService
{
    public async Task<Guid> QueueCreateCustomer(
        string firstName,
        string surname
    )
    {
        var newCustomerId = Guid.NewGuid();

        await _kafkaProducer.SendEvent(
            "customer-create",
            new CustomerDto(
                newCustomerId,
                firstName,
                surname
            )
        );

        return newCustomerId;
    }

    public async Task CreateCustomer(
        CustomerDto customerDto
    ) => await _customerData.CreateCustomer(
        customerDto
    );

    public async Task<CustomerDto[]> GetAllCustomers()
        => await _customerData.GetAllCustomers();

    public async Task<CustomerDto> GetCustomerByIdentifier(
        Guid customerIdentifier
    ) => await _customerData.GetCustomerByIdentifier(
        customerIdentifier
    );
}