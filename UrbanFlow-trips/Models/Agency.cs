namespace UrbanFlow_trips.Models;

public class Agency
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; }
    public string TimeZone { get; set; }
    
    public ICollection<Routes> Routes { get; set; }
    public ICollection<Stop_Times> Stops { get; set; } 


}