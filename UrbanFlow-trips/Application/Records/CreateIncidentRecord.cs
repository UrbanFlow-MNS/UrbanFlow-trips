using System.Text.Json.Serialization;

namespace UrbanFlow_trips.Application.Records;

public record CreateIncidentRecord
{
    [JsonPropertyName("incidentId")]
    public int IncidentId { get; init; }

    [JsonPropertyName("siteId")]
    public int SiteId { get; init; }

    [JsonPropertyName("estimateDuration")]
    public int EstimateDuration { get; init; }

    [JsonPropertyName("priority")]
    public string Priority { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; }

    [JsonPropertyName("affectedStopIds")]
    public List<int> AffectedStopIds { get; init; }

    [JsonPropertyName("affectedRouteIds")]
    public List<int> AffectedRouteIds { get; init; }
}