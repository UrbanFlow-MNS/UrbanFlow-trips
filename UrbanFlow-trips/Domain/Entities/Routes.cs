namespace UrbanFlow_trips.Models;

public class Routes
{
    public int RouteId { get; set; }
    public int AgencyId { get; set; }
    public required string RouteShortName { get; set; }
    public required string RouteLongName { get; set; }
    public int RouteTypeId { get; set; }
    
    public required ICollection<Trip> Trips { get; set; }
}