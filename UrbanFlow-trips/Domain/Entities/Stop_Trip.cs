namespace UrbanFlow_trips.Models;

public class Stop_Trip
{
    public int TripId { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public TimeOnly? DepartureTime  { get; set; }
    public int StopId { get; set; }
    public int StopSequence { get; set; }
    
    public Trip Trip { get; set; }
    public Stop_Times Stop { get; set; }
}