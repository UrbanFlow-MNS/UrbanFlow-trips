namespace UrbanFlow_trips.Infrastructure.Exceptions;

public class MessagingPublishException : System.Exception
{
    public string QueueName { get; }
    
    public MessagingPublishException(string queueName, System.Exception innerException) : base($"Failed to publish message to queue '{queueName}'", innerException)
    {
        QueueName = queueName;
    }
}