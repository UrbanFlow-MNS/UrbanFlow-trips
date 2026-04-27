namespace UrbanFlow_trips.Models;

public class Incident
{
    public int IncidentId { get; set; }
    public int RouteId { get; set; }
    public int EstimateDuration { get; set; }
    public DateOnly CreatedAt { get; set; }
}