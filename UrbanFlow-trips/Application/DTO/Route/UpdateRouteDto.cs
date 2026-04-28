namespace UrbanFlow_trips.Application.DTO.Route;

public class UpdateRouteDto
{
    public required string RouteShortName { get; set; }
    public required string RouteLongName { get; set; }
    public int RouteTypeId { get; set; }
}