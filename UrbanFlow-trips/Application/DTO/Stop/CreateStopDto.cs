namespace UrbanFlow_trips.Application.DTO.Stop;

public class CreateStopDto
{
    public required string StopName { get; set; }
    public required decimal StopLat { get; set; }
    public required decimal StopLong { get; set; }
    public int AgencyId { get; set; }
}