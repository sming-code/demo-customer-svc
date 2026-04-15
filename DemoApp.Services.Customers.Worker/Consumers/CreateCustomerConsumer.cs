using System.Text.Json;

namespace DemoApp.Services.Customers.Worker;
using Models;

public class CreateCustomerConsumer : IMinimalConsumer
{
    public void Consume(KafkaApplicationBuilder builder) =>
        builder.MapConsumer(
            "customer-create",
            async (
                [FromEventValue] CustomerDto customerDto,
                ICustomerService customerService,
                ILogger<CreateCustomerConsumer> logger
            ) =>
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "Received message on customer-create topic, with value '{EventValue}'",
                        JsonSerializer.Serialize(customerDto)
                    );                    
                }

                await customerService.CreateCustomer(
                    customerDto
                );

                logger.LogInformation(
                    "Customer created with id {Customer Id}",
                    customerDto.CustomerIdentifier
                );

                return KafkaEventResult.Complete;
            }
        );
}