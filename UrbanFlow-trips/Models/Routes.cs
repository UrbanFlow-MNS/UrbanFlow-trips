namespace UrbanFlow_trips.Models;

public class Routes
{
    public int RouteId { get; set; }
    public int AgencyId { get; set; }
    public string RouteShortName { get; set; }
    public string RouteLongName { get; set; }
    public int RouteTypeId { get; set; }
    
    public Agency Agency { get; set; }
    public RouteType RouteType { get; set; }
    public ICollection<Trip> Trips { get; set; }
}