namespace UrbanFlow_trips.Application.DTO.Stop;

public class GetStopDto
{
    public int StopId { get; set; }
    public int AgencyId { get; set; }
    
    public string? StopName { get; set; }
    public decimal StopLat { get; set; }
    public decimal StopLong { get; set; }
}