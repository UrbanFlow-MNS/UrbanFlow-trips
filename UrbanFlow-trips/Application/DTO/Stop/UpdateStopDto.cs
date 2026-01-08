namespace UrbanFlow_trips.DTO;

public class UpdateStopDto
{
    public required string StopName { get; set; }
    public required decimal StopLat { get; set; }
    public required decimal StopLong { get; set; }
}