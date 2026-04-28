using UrbanFlow_trips.Application.DTO.Calendar;

namespace UrbanFlow_trips.Domain.Interfaces;

public interface ICalendarRepository
{
    Task CreateCalendarAsync(CreateCalendarDto calendarDto);
    Task<List<GetCalendarDto>> GetAllCalendarsAsync();
    Task<bool> CalendarExistsAsync(int id, CancellationToken cancellationToken = default);
}