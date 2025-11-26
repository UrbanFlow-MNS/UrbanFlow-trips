namespace UrbanFlow_trips.DTO;

public class CreateRouteDTO
{
    public int AgencyId { get; set; }
    public string RouteShortName { get; set; }
    public string RouteLongName { get; set; }
    public int RouteTypeId { get; set; }
}