namespace UrbanFlow_trips.DTO;

public class GetTripDetailsDto
{
    public int TripId { get; set; }
    public IEnumerable<GetStopDetailsDto> Stops { get; set; } 

}