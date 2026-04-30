namespace UrbanFlow_trips.Domain.Entities;

public class Stop_Times
{
    public int StopId { get; set; }
    public int AgencyId { get; set; }
    public required string StopName { get; set; }
    public decimal StopLat { get; set; }
    public decimal StopLong { get; set; }
    
    public ICollection<Stop_Trip> StopTrips { get; set; }
}