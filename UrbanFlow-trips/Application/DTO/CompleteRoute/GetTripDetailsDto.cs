namespace UrbanFlow_trips.DTO;

public class GetTripDetailsDto
{
    public int TripId { get; set; }
    public List<GetStopDetailsDto> Stops { get; set; } = new();

}