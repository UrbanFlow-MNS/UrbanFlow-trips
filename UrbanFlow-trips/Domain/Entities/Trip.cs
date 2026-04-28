namespace UrbanFlow_trips.Domain.Entities;

public class Trip
{
    public int TripId { get; set; }
    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public required string TripHeadsign { get; set; }
    
    public required Routes Routes { get; set; }
    public required Calendar Calendar { get; set; }
    public ICollection<Stop_Trip> StopTrips { get; set; }
}