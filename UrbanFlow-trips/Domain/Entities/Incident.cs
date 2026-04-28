namespace UrbanFlow_trips.Domain.Entities;

public class Incident
{
    public int IncidentId { get; set; }
    public int RouteId { get; set; }
    public int EstimateDuration { get; set; }
    public DateOnly CreatedAt { get; set; }
}