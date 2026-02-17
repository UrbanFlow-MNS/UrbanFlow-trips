using Microsoft.AspNetCore.Mvc;

namespace UrbanFlow_trips.Controllers;

[ApiController]
[Route("metrics")]
public class PrometheusController(IPrometheusService prometheusService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetMetrics()
    {
        var metrics = await prometheusService.GetMetricsAsync();
        
        return Content(metrics, "text/plain; version=0.0.4; charset=utf-8");
    }
}