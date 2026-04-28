using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class CalendarRepository(TripsDbContext dbContext, IMapper mapper) : ICalendarRepository
{
    public async Task CreateCalendarAsync(CreateCalendarDto calendarDto)
    {
        ArgumentNullException.ThrowIfNull(calendarDto);

        var calendar = mapper.Map<Calendar>(calendarDto);
        await dbContext.Calendars.AddAsync(calendar);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GetCalendarDto>> GetAllCalendarsAsync()
    {
        var calendars = await dbContext.Calendars.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetCalendarDto>>(calendars);
    }
    
    public async Task<bool> CalendarExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Calendars.AnyAsync(x => x.ServiceId == id, cancellationToken);
    }
}