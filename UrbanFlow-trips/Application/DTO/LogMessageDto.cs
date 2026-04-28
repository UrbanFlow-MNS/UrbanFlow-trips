namespace UrbanFlow_trips.Application.DTO;

public class LogMessageDto
{
    public required string MicroserviceName { get; set; }
    public required int CodeOfEvent { get; set; }
    public required string Event { get; set; }
}