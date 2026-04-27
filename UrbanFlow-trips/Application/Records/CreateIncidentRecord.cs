using System.Text.Json.Serialization;

namespace UrbanFlow_trips.Application.Records;

public record CreateIncidentRecord
{
    public int IncidentId { get; init; }
    public int SiteId { get; init; }
    public int EstimateDuration { get; init; }
    public string Priority { get; init; }
    public string Status { get; init; }
    public List<int> AffectedStopIds { get; init; }
    public List<int> AffectedRouteIds { get; init; }
}