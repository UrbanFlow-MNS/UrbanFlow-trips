namespace UrbanFlow_trips.DTO;

public class UpdateStopTripDTO
{
    public TimeOnly ArrivalTime { get; set; }
    public TimeOnly? DepartureTime  { get; set; }
}