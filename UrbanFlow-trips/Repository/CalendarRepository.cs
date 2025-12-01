using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class CalendarRepository(TripsDbContext dbContext)
{
    public async Task CreateCalendarAsync(CreateCalendarDTO calendarDto)
    {
        Calendar calendar = new Calendar()
        {
            Monday = calendarDto.Monday,
            Tuesday = calendarDto.Tuesday,
            Wednesday = calendarDto.Wednesday,
            Thursday = calendarDto.Thursday,
            Friday = calendarDto.Friday,
            Saturday = calendarDto.Saturday,
            Sunday = calendarDto.Sunday,
            StartDate = calendarDto.StartDate
        };
        
        await dbContext.Calendars.AddAsync(calendar);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Calendar>> GetAllCalendarsAsync()
    {
        return await dbContext.Calendars.ToListAsync();
    }
}