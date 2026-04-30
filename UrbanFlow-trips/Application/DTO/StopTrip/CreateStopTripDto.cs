namespace UrbanFlow_trips.Application.DTO.StopTrip;

public class CreateStopTripDto
{
    public int TripId { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public TimeOnly? DepartureTime  { get; set; }
    public int StopId { get; set; }
    public int StopSequence { get; set; }

}