using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.MsTest.TestHelpers;

public static class EntityFactory
{
    public static Routes CreateRoute(int routeId = 0, int agencyId = 1, int routeTypeId = 2)
        => new()
        {
            RouteId = routeId,
            AgencyId = agencyId,
            RouteShortName = "A",
            RouteLongName = "Ligne A",
            RouteTypeId = routeTypeId,
            Trips = new List<Trip>()
        };

    public static Calendar CreateCalendar(int serviceId = 0)
        => new()
        {
            ServiceId = serviceId,
            Monday = true,
            Tuesday = true,
            Wednesday = false,
            Thursday = true,
            Friday = true,
            Saturday = false,
            Sunday = false,
            StartDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 12, 31),
            Trips = new List<Trip>()
        };

    public static Stop_Times CreateStop(int stopId = 0, string name = "Gare Centrale")
        => new()
        {
            StopId = stopId,
            AgencyId = 1,
            StopName = name,
            StopLat = 48.85m,
            StopLong = 2.35m,
            StopTrips = new List<Stop_Trip>()
        };

    public static Trip CreateTrip(Routes route, Calendar calendar, int tripId = 0)
        => new()
        {
            TripId = tripId,
            RouteId = route.RouteId,
            ServiceId = calendar.ServiceId,
            TripHeadsign = "Direction Centre",
            Routes = route,
            Calendar = calendar,
            StopTrips = new List<Stop_Trip>()
        };

    public static Stop_Trip CreateStopTrip(Trip trip, Stop_Times stop, int sequence = 1)
        => new()
        {
            TripId = trip.TripId,
            StopId = stop.StopId,
            ArrivalTime = new TimeOnly(8, 0),
            DepartureTime = new TimeOnly(7, 55),
            StopSequence = sequence,
            Trip = trip,
            Stop = stop
        };
}
