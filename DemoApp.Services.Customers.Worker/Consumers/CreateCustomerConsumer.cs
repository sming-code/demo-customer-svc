using System.Text.Json;
using SmingCode.Utilities.Kafka;
using SmingCode.Utilities.Kafka.MinimalConsumers;

namespace DemoApp.Services.Customers.Worker;
using Models;

public class CreateCustomerConsumer : IMinimalConsumer
{
    public void Consume(KafkaApplicationBuilder builder) =>
        builder.MapConsumer(
            "customer-create",
            async (
                [FromEventValue] CreateCustomerModel customerModel,
                ICustomerService customerService,
                ILogger<CreateCustomerConsumer> logger
            ) =>
            {
                logger.LogInformation(
                    "Received message on customer-create topic, with value '{EventValue}'",
                    JsonSerializer.Serialize(customerModel)
                );

                var newCustomerId = await customerService.CreateCustomer(
                    customerModel.FirstName,
                    customerModel.Surname
                );

                logger.LogInformation(
                    "Customer created with id {Customer Id}",
                    newCustomerId
                );

                return KafkaEventResult.Complete;
            }
        ).CreateTopicIfNotExists();
}