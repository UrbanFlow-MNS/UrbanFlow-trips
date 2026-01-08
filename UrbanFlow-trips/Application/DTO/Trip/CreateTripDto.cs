namespace UrbanFlow_trips.DTO;

public class CreateTripDto
{
    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public string? TripHeadsign { get; set; }
    public List<CreateStopTripDto> CreateStopTrips { get; set; } = new();
}