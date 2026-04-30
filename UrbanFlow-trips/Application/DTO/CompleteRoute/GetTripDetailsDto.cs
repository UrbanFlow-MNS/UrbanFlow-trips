namespace UrbanFlow_trips.Application.DTO.CompleteRoute;

public class GetTripDetailsDto
{
    public int TripId { get; set; }
    public IEnumerable<GetStopDetailsDto> Stops { get; set; } 

}