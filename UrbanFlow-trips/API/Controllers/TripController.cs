using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController(ITripRepository _tripRepository, IStopTripRepository _stopTripRepository) : Controller
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO tripDto)
    {
        await _tripRepository.CreateTripAsync(tripDto);
        return Ok(new
        {
            message = "Trip created successfully"
        });
    }
    
    
    [HttpPut("update/{stopId}/{tripId}")]
    public async Task<IActionResult> UpdateTrip([FromBody] UpdateStopTripDTO stopTripDto, int stopId, int tripId)
    {
        await _stopTripRepository.UpdateStopTripAsync(stopId, tripId, stopTripDto);
        return Ok(new
        {
            message = "Trip updated successfully"
        });
    }
    
}