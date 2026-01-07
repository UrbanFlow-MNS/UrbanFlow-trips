using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteTypeController(IRouteTypeRepository routesTypeRepository) : Controller
{
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateRouteTypeDTO routeDto)
    {
        await routesTypeRepository.CreateRouteTypeAsync(routeDto);
        return Ok(new
        {
            message = "Route type created successfully"
        });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAgencies()
    {
        return Ok(await routesTypeRepository.GetAllRouteTypesAsync());
    }
}