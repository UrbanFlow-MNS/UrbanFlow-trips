namespace UrbanFlow_trips.Models;

public class Trip
{
    public int TripId { get; set; }
    public int RouteId { get; set; }
    public int ServiceId { get; set; }
    public string TripHeadsign { get; set; }
    
    public Routes Routes { get; set; }
    public Calendar Calendar { get; set; }
    public ICollection<Stop_Trip> StopTrips { get; set; }
}