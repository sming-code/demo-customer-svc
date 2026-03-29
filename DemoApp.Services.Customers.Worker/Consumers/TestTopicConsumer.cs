using SmingCode.Utilities.Kafka;
using SmingCode.Utilities.Kafka.MinimalConsumers;

namespace DemoApp.Services.Customers.Worker;

public class TestTopicConsumer : IMinimalConsumer
{
    public void Consume(KafkaApplicationBuilder builder) =>
        builder.MapConsumer(
            "test_topic",
            async (
                [FromEventValue] string testEventValue,
                IKafkaConsumerDefinition kafkaConsumerDefinition,
                ILogger<TestTopicConsumer> logger
            ) =>
            {
                logger.LogInformation(
                    "Received message on test-topic topic, with value '{EventValue}'",
                    testEventValue
                );

                return KafkaEventResult.Incomplete;
            }
        ).CreateTopicIfNotExists();
}