using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutesController(IRoutesRepository _routesRepository) : Controller
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateRouteDTO routeDTO)
    {
        _routesRepository.CreateRouteAsync(routeDTO);
        return Ok(new
        {
            message = "Route created successfully"
        });
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterRoutes([FromQuery] RouteFilterDTO filter)
    {
        var routes = await _routesRepository.GetRoutesFilter(filter);
        if (!routes.Any()) return NotFound();
        return Ok(routes);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAgencies()
    {
        return Ok(await _routesRepository.GetAllRoutesAsync());
    }

    [HttpGet("getDetails/{id}")]
    public async Task<IActionResult> GetRouteDetails(int id)
    {
        return Ok(await _routesRepository.GetCompleteRouteAsync(id));
    }
}