using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StopsController(IStopRepository stopRepository) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateStop(CreateStopDto stopDto)
    {
        await stopRepository.CreateStopAsync(stopDto);
        return Ok(new
        {
            message = "Stop created successfully"
        });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllStops()
    {
        return Ok(await stopRepository.GetAllStopsAsync());
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateStop(UpdateStopDto stopDto, int id)
    {
        await stopRepository.UpdateStopAsync(id, stopDto);
        return Ok(new
        {
            message = "Stop updated successfully"
        });
    }
}