using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;
using UrbanFlow_trips.Services;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutesController(IRoutesRepository routesRepository, IGetAdjustedRoutesUseCase useCase) : ControllerBase
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoutes()
    {
        return Ok(await routesRepository.GetAllRoutesAsync());
    }

    [HttpGet("getDetails/{id}")]
    public async Task<IActionResult> GetRouteDetails(int id)
    {
        return Ok(await routesRepository.GetCompleteRouteByIdAsync(id));
    }

    [HttpGet("getAllCompleteRoutes")]
    public async Task<IActionResult> GetAllCompleteRoutes()
    {
        return Ok(await routesRepository.GetAllCompleteRoutesAsync());
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoutes([FromQuery] RouteFilterDto filter)
    {
        var routes = await useCase.ExecuteAsync(filter);
        return Ok(routes);
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