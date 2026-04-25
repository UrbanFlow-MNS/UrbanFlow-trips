using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Service;

namespace UrbanFlow_trips.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController(IRabbitMQService mq, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> SendLog(LogMessageDto messageDto)
    {
        await mq.PublishAsync("LOGS_QUEUE_IN", messageDto, "logs_created");
        return Ok("Log envoyé !");
    }
    
    [HttpPost("test")]
    public async Task TestLogs(LogMessageDto messageDto)
    {
        await publishEndpoint.Publish(messageDto);
    }    
    
}