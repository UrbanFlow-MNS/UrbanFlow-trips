namespace UrbanFlow_trips.Application.DTO.Route;

public class GetRouteDto()
{
    public int RouteId { get; set; }
    public int AgencyId { get; set; }
    
    public string RouteTypeName { get; set; } 

    public string? RouteShortName { get; set; }
    public string? RouteLongName { get; set; }
}