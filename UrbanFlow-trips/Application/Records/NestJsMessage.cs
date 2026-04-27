using System.Text.Json.Serialization;
using UrbanFlow_trips.Application.Records;

public record NestJsWrapper()
{
    public string Pattern { get; set; }
    public CreateIncidentRecord Data { get; set; }
}