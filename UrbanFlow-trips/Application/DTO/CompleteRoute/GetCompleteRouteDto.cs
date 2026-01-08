namespace UrbanFlow_trips.DTO;

public class GetCompleteRouteDto
{
    public required int RouteId { get; set; }
    public required string RouteShortName { get; set; }
    public required string RouteLongName { get; set; }
    public required string RouteTypeName { get; set; }

    public List<GetTripDetailsDto> Trips { get; set; } = new();
}