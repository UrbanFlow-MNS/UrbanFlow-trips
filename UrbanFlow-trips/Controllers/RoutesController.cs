using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutesController : Controller
{
    private readonly RoutesRepository _routesRepository;
    
    public RoutesController(RoutesRepository routesRepository)
    {
        _routesRepository = routesRepository;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateRouteDTO routeDTO)
    {
        await _routesRepository.CreateRouteAsync(routeDTO);
        return Ok(new
        {
            message = "Route created successfully"
        });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAgencies()
    {
        return Ok(await _routesRepository.GetAllRoutesAsync());
    }
}