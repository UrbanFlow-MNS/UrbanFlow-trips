using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface ICalendarRepository
{
    Task CreateCalendarAsync(CreateCalendarDTO calendarDto);
    Task<List<GetCalendarDTO>> GetAllCalendarsAsync();
}