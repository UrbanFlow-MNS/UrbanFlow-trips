using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class CalendarRepository(TripsDbContext dbContext, IMapper _mapper)
{
    public async Task CreateCalendarAsync(CreateCalendarDTO calendarDto)
    {
        var calendar = _mapper.Map<Calendar>(calendarDto);
        await dbContext.Calendars.AddAsync(calendar);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GetCalendarDTO>> GetAllCalendarsAsync()
    {
        var calendar = await dbContext.Calendars.ToListAsync();
        return _mapper.Map<List<GetCalendarDTO>>(calendar);
    }
}