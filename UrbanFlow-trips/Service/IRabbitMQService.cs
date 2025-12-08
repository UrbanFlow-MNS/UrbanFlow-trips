namespace UrbanFlow_trips.Service;

public interface IRabbitMQService
{
    Task PublishAsync(string queueName, object message, string eventPattern);
}