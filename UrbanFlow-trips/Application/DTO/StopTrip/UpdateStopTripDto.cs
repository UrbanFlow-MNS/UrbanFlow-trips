namespace UrbanFlow_trips.DTO;

public class UpdateStopTripDto
{
    public required TimeOnly ArrivalTime { get; set; }
    public TimeOnly? DepartureTime  { get; set; }
}