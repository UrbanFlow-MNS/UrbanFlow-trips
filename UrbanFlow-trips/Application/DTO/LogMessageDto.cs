namespace UrbanFlow_trips.DTO;

public class LogMessageDto
{
    public required string MicroserviceName { get; set; }
    public required int CodeOfEvent { get; set; }
    public required string Event { get; set; }
}