namespace UrbanFlow_trips.Application.DTO.Route;

public class CreateRouteDto
{
    public required int AgencyId { get; set; }
    public required string RouteShortName { get; set; }
    public required string RouteLongName { get; set; }
    public required int RouteTypeId { get; set; }
}