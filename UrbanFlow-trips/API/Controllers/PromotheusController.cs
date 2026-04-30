using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.Domain.Service;

namespace UrbanFlow_trips.API.Controllers;

[ApiController]
[Route("metrics")]
public class PrometheusController(PrometheusService prometheusService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetMetrics()
    {
        var metrics = await prometheusService.GetMetricsAsync();
        
        return Content(metrics, "text/plain; version=0.0.4; charset=utf-8");
    }
}