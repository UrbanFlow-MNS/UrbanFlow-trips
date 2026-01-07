using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController(ITripRepository tripRepository, IStopTripRepository stopTripRepository) : Controller
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO tripDto)
    {
        await tripRepository.CreateTripAsync(tripDto);
        return Ok(new
        {
            message = "Trip created successfully"
        });
    }
    
    
    [HttpPut("updateHourly/{stopId}/{tripId}")]
    public async Task<IActionResult> UpdateHourlyTrip([FromBody] UpdateStopTripDTO stopTripDto, int stopId, int tripId)
    {
        await stopTripRepository.UpdateStopTripAsync(stopId, tripId, stopTripDto);
        return Ok(new
        {
            message = "Trip hours updated successfully"
        });
    }
    
    [HttpPut("updateHourly/{tripId}")]
    public async Task<IActionResult> UpdateTripService([FromBody] UpdateTripServiceDTO service, int tripId)
    {
        await tripRepository.UpdateTripService(tripId, service);
        return Ok(new
        {
            message = "Trip's service updated successfully"
        });
    }
    
    
    
}