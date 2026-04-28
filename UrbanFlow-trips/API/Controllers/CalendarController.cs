using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CalendarController(ICalendarRepository calendarRepository) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateCalendar(CreateCalendarDto calendarDto)
    {
        await calendarRepository.CreateCalendarAsync(calendarDto);
        return Ok(new
        {
            message = "Calendar created successfully"
        });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCalendars()
    {
        return Ok(await calendarRepository.GetAllCalendarsAsync());
    }
    
}