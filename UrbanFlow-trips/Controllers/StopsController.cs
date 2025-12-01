using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StopsController : Controller
{
    private readonly StopRepository _stopRepository;

    [HttpPost("create")]
    public async Task<IActionResult> CreateStop(CreateStopDTO stopDto)
    {
        await _stopRepository.CreateStopAsync(stopDto);
        return Ok(new
        {
            message = "Stop created successfully"
        });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllStops()
    {
        return Ok(await _stopRepository.GetAllStopsAsync());
    }
}