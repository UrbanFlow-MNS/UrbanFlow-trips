namespace UrbanFlow_trips.DTO;

public class CreateTripDTO
{
    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public string TripHeadsign { get; set; }
    public List<CreateStopTripDTO> CreateStopTrips { get; set; }
}