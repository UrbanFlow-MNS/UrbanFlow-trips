namespace UrbanFlow_trips.DTO;

public class GetTripDetailsDTO
{
    public int TripId { get; set; }
    public List<GetStopDetailsDTO> Stops { get; set; } 

}