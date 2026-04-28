namespace UrbanFlow_trips.Infrastructure.Messaging;

public interface IRabbitMQService
{
    Task PublishAsync(string queueName, object message, string eventPattern);
}