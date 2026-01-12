using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutesController(IRoutesRepository routesRepository) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateRouteDto routeDto)
    {
        await routesRepository.CreateRouteAsync(routeDto);
        return Ok(new
        {
            message = "Route created successfully"
        });
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterRoutes([FromQuery] RouteFilterDto filter)
    {
        var routes = await routesRepository.GetRoutesFilter(filter);
        if (!routes.Any()) return NotFound();
        return Ok(routes);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAgencies()
    {
        return Ok(await routesRepository.GetAllRoutesAsync());
    }

    [HttpGet("getDetails/{id}")]
    public async Task<IActionResult> GetRouteDetails(int id)
    {
        return Ok(await routesRepository.GetCompleteRouteAsync(id));
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateRoute(UpdateRouteDto routeDto, int id)
    {
        await routesRepository.UpdateRouteAsync(id, routeDto);
        return Ok(new
        {
            message = "Route updated successfully"
        });
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAgency(int id)
    {
        await routesRepository.DeleteRouteAsync(id);
        return Ok(new
        {
            message = "Route deleted successfully"
        });
    }
}