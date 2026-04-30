using UrbanFlow_trips.Application.DTO.StopTrip;

namespace UrbanFlow_trips.Application.DTO.Trip;

public class CreateTripDto
{
    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public string? TripHeadsign { get; set; }
    public List<CreateStopTripDto> CreateStopTrips { get; set; } = new();
}