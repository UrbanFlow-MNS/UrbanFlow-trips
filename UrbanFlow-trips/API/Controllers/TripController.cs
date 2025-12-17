using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController(ITripRepository _tripRepository) : Controller
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency([FromBody] CreateTripDTO tripDto)
    {
        await _tripRepository.CreateTripAsync(tripDto);
        return Ok(new
        {
            message = "Trip created successfully"
        });
    }
    
}