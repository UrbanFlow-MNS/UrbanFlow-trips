using MassTransit;
using UrbanFlow_trips.Application.DTO;

namespace UrbanFlow_trips.API.Consumers;

public class PostLogsConsumer : IConsumer<LogMessageDto>
{
    public Task Consume(ConsumeContext<LogMessageDto> context)
    {
        Console.WriteLine("PostLogsConsumer");
        Console.WriteLine(context.Message);
        return Task.CompletedTask;
    }
}