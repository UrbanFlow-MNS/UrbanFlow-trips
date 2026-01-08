namespace UrbanFlow_trips.DTO;

public class UpdateRouteDto
{
    public required string RouteShortName { get; set; }
    public required string RouteLongName { get; set; }
    public int RouteTypeId { get; set; }
}