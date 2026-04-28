namespace UrbanFlow_trips.Application.Records;

public record NestJsWrapper()
{
    public required string Pattern { get; set; }
    public required CreateIncidentRecord Data { get; set; }
}