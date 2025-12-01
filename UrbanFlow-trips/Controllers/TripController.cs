using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController : Controller
{
    private readonly TripRepository _tripRepository;
    
    public TripController(TripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateTripDTO tripDto)
    {
        await _tripRepository.CreateTripAsync(tripDto);
        return Ok(new
        {
            message = "Trip created successfully"
        });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTrips()
    {
        return Ok(await _tripRepository.GetAllTripsAsync());
    }
    
}