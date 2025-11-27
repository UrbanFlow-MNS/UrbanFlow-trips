using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteTypeController : Controller
{
    private readonly RouteTypeRepository _routesTypeRepository;
    
    public RouteTypeController(RouteTypeRepository routesTypeRepository)
    {
        _routesTypeRepository = routesTypeRepository;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateRouteTypeDTO routeDTO)
    {
        await _routesTypeRepository.CreateRouteTypeAsync(routeDTO);
        return Ok(new
        {
            message = "Agency created successfully"
        });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAgencies()
    {
        return Ok(await _routesTypeRepository.GetAllRouteTypesAsync());
    }
}