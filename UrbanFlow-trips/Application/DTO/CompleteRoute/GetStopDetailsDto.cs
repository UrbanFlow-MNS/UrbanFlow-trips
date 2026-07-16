namespace UrbanFlow_trips.Application.DTO.CompleteRoute;

public class GetStopDetailsDto
{
    public int StopId { get; set; }
    public string StopName { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public double CurrentArrivalTime { get; set; }
    public double BaseArrivalTime { get; set; }
    public int Delay { get; set; }
    public int SequenceOrder { get; set; }
}