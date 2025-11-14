namespace UrbanFlow_trips.Models;

public class Routes
{
    public int route_id { get; set; }
    public int agency_id { get; set; }
    public string route_short_name { get; set; }
    public string route_long_name { get; set; }
    public int route_type_id { get; set; }
}