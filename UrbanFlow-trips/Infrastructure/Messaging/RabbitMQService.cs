using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UrbanFlow_trips.Exceptions;
using UrbanFlow_trips.Options;
using UrbanFlow_trips.Service;

namespace UrbanFlow_trips.Infrastucture.Messaging;

public class RabbitMQService : IRabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public RabbitMQService(IOptions<RabbitMQOptions> options)
    {
        var option = options.Value;
        _factory = new ConnectionFactory()
        {
            HostName = option.HostName,
            UserName = option.UserName,
            Password = option.Password
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    
    public async Task PublishAsync(string queueName, object message, string eventPattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentException.ThrowIfNullOrWhiteSpace(eventPattern);
        ArgumentNullException.ThrowIfNull(message);
        
        try
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queueName, 
                durable: true,  // Persistance
                exclusive: false, 
                autoDelete: false
            );
            
    
            var json = JsonSerializer.Serialize(message, _jsonOptions);
            var body = Encoding.UTF8.GetBytes(json);

    
            var properties = new BasicProperties
            {
                Persistent = true,  // Messages persistants
                ContentType = "application/json"
            };
    
            await channel.BasicPublishAsync(
                exchange: string.Empty, 
                routingKey: queueName, 
                mandatory: true,
                basicProperties: properties,
                body: body
            );
        }
        catch (Exception ex)
        {
            throw new MessagingPublishException($"Failed to publish to {queueName}", ex);
        }
    }
}