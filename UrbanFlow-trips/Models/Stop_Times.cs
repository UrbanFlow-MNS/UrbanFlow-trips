namespace UrbanFlow_trips.Models;

public class Stop_Times
{
    public int RouteId { get; set; }
    public string StopName { get; set; }
    public decimal StopLat { get; set; }
    public decimal StopLong { get; set; }
}