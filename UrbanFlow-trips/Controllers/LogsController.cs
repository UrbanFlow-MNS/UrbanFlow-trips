using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Service;

namespace UrbanFlow_trips.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController : Controller
{
    private readonly IRabbitMQService _mq;

    public LogsController(IRabbitMQService mq)
    {
        _mq = mq;
    }

    [HttpPost("create")]
    public async Task<IActionResult> SendLog(LogMessageDTO messageDto)
    {
        await _mq.PublishAsync("LOGS_QUEUE_IN", messageDto, "logs_created");
        return Ok("Log envoyé !");
    }
}