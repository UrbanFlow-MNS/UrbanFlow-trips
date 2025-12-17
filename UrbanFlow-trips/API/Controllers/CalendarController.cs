using Microsoft.AspNetCore.Mvc;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CalendarController(ICalendarRepository _calendarRepository) : Controller
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateCalendar(CreateCalendarDTO calendarDto)
    {
        await _calendarRepository.CreateCalendarAsync(calendarDto);
        return Ok(new
        {
            message = "Calendar created successfully"
        });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCalendars()
    {
        return Ok(await _calendarRepository.GetAllCalendarsAsync());
    }
    
}