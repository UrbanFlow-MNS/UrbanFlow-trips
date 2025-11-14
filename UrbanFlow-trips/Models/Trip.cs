namespace UrbanFlow_trips.Models;

public class Trip
{
    public int TripId { get; set; }
    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public string TripHeadsign { get; set; }
}