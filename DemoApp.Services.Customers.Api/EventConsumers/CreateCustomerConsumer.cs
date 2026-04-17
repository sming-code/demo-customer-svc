using System.Text.Json;

namespace DemoApp.Services.Customers.Api.EventConsumers;

public class CreateCustomerConsumer : IMinimalConsumer
{
    public void Consume(IServiceCollection services) =>
        services.MapConsumer(
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
        ).CreateTopicIfNotExists();
}