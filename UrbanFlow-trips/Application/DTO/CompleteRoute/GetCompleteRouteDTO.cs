namespace UrbanFlow_trips.DTO;

public class GetCompleteRouteDTO
{
    public int RouteId { get; set; }
    public string RouteShortName { get; set; }
    public string RouteLongName { get; set; }
    public string RouteTypeName { get; set; }

    public List<GetTripDetailsDTO> Trips { get; set; } = new();
}