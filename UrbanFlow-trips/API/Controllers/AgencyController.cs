using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgencyController : Controller
{
    private readonly AgencyRepository _agencyRepository;
    
    public AgencyController(AgencyRepository agencyRepository)
    {
        _agencyRepository = agencyRepository;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAgency(CreateAgencyDTO agencyDto)
    {
        await _agencyRepository.CreateAgencyAsync(agencyDto);
        return Ok(new
        {
            message = "Agency created successfully"
        });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAgencies()
    {
        return Ok(await _agencyRepository.GetAllAgenciesAsync());
    }
}