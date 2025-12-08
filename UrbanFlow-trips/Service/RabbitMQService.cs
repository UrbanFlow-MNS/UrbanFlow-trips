using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace UrbanFlow_trips.Service;

public class RabbitMQService : IRabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public RabbitMQService()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "user",
            Password = "password"
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    
    public async Task PublishAsync(string queueName, object message, string eventPattern)
    {
        using var connection = await _factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queueName, durable: false, exclusive: false, autoDelete: false);

        message = new
        {
            pattern = eventPattern,
            data = message
        };
        
        var json = JsonSerializer.Serialize(message, _jsonOptions);
        var body = Encoding.UTF8.GetBytes(json);
        
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
    }
}