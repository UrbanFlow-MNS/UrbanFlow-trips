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

    [HttpPost("log")]
    public async Task<IActionResult> SendLog(LogMessage message)
    {
        await _mq.PublishAsync("LOGS_QUEUE_IN", message, "logs_created");
        return Ok("Log envoyé !");
    }
}