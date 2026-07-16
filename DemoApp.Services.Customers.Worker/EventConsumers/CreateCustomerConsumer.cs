using System.Text.Json;

namespace DemoApp.Services.Customers.Worker.EventConsumers;
using Events;

public class CreateCustomerConsumer : IMinimalConsumer
{
    public void Consume(IServiceCollection services) =>
        services.MapConsumer(
            "customer-create",
            async (
                [FromEventValue] CustomerEvent customerEvent,
                ICustomerService customerService,
                ILogger<CreateCustomerConsumer> logger
            ) =>
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "Received message on customer-create topic, with value '{EventValue}'",
                        JsonSerializer.Serialize(customerEvent)
                    );                    
                }

                await customerService.CreateCustomer(
                    customerEvent.CustomerIdentifier,
                    customerEvent.FirstName,
                    customerEvent.Surname
                );

                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "Customer created with id {Customer Id}",
                        customerEvent.CustomerIdentifier
                    );
                }

                return KafkaEventResult.Complete;
            }
        ).CreateTopicIfNotExists();
}