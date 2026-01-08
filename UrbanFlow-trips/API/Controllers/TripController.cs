using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController(ITripRepository tripRepository, IStopTripRepository stopTripRepository) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto tripDto)
    {
        await tripRepository.CreateTripAsync(tripDto);
        return Ok(new
        {
            message = "Trip created successfully"
        });
    }
    
    
    [HttpPut("updateHourly/{stopId}/{tripId}")]
    public async Task<IActionResult> UpdateHourlyTrip([FromBody] UpdateStopTripDto stopTripDto, int stopId, int tripId)
    {
        await stopTripRepository.UpdateStopTripAsync(stopId, tripId, stopTripDto);
        return Ok(new
        {
            message = "Trip hours updated successfully"
        });
    }
    
    [HttpPut("updateHourly/{tripId}")]
    public async Task<IActionResult> UpdateTripService([FromBody] UpdateTripServiceDto service, int tripId)
    {
        await tripRepository.UpdateTripService(tripId, service);
        return Ok(new
        {
            message = "Trip's service updated successfully"
        });
    }
    
    
    
}