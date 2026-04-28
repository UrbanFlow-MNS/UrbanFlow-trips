namespace UrbanFlow_trips.Application.DTO.StopTrip;

public class UpdateStopTripDto
{
    public required TimeOnly ArrivalTime { get; set; }
    public TimeOnly? DepartureTime  { get; set; }
}