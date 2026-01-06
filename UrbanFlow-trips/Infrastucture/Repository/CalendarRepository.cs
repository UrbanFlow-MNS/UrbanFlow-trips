using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class CalendarRepository(TripsDbContext dbContext, IMapper mapper) : ICalendarRepository
{
    public async Task CreateCalendarAsync(CreateCalendarDTO calendarDto)
    {
        ArgumentNullException.ThrowIfNull(calendarDto);

        var calendar = mapper.Map<Calendar>(calendarDto);
        await dbContext.Calendars.AddAsync(calendar);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GetCalendarDTO>> GetAllCalendarsAsync()
    {
        var calendars = await dbContext.Calendars.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetCalendarDTO>>(calendars);
    }
}