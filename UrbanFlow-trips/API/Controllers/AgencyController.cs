using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgencyController(IAgencyRepository _agencyRepository) : Controller
{
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
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAgency(UpdateAgencyDTO agencyDto, int id)
    {
        await _agencyRepository.UpdateAgencyAsync(id, agencyDto);
        return Ok(new
        {
            message = "Agency updated successfully"
        });
    }
}