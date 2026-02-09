using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface ICalendarRepository
{
    Task CreateCalendarAsync(CreateCalendarDto calendarDto);
    Task<List<GetCalendarDto>> GetAllCalendarsAsync();
    Task<bool> CalendarExistsAsync(int id, CancellationToken cancellationToken = default);
}