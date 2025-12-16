namespace UrbanFlow_trips.DTO;

public class GetStopDetailsDTO
{
    public int StopId { get; set; }
    public string StopName { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public int SequenceOrder { get; set; }
}