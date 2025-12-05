namespace UrbanFlow_trips.DTO;

public class CreateStopTripDTO
{
    public int TripId { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public TimeOnly DepartureTime  { get; set; }
    public int StopId { get; set; }
    public int StopSequence { get; set; }

}