namespace UrbanFlow_trips.DTO;

public class GetRouteDto
{
    public int RouteId { get; set; }
    public int AgencyId { get; set; }
    public int RouteTypeId { get; set; }


    public string? RouteShortName { get; set; }
    public string? RouteLongName { get; set; }
}