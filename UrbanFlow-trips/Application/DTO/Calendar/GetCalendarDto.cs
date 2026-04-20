namespace UrbanFlow_trips.DTO;

public class GetCalendarDto
{
    public int ServiceId { get; set; }
    public bool Monday { get; init; }
    public bool Tuesday { get; init; }
    public bool Wednesday { get; init; }
    public bool Thursday { get; init; }
    public bool Friday { get; init; }
    public bool Saturday { get; init; }
    public bool Sunday { get; init; }
    public DateOnly EndDate { get; set; }

}