using AutoMapper;
using UrbanFlow_trips.Application.DTO.Calendar;
using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Application.DTO.Stop;
using UrbanFlow_trips.Application.DTO.StopTrip;
using UrbanFlow_trips.Application.DTO.Trip;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Unit.Mapping;

[TestClass]
public class MappingProfilesTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();

    [TestMethod]
    public void Map_CreateCalendarDto_To_Calendar()
    {
        var dto = new CreateCalendarDto
        {
            Monday = true,
            Tuesday = false,
            Wednesday = true,
            Thursday = false,
            Friday = true,
            Saturday = false,
            Sunday = true,
            StartDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 6, 30)
        };

        var entity = Mapper.Map<Calendar>(dto);

        Assert.IsTrue(entity.Monday);
        Assert.IsFalse(entity.Tuesday);
        Assert.IsTrue(entity.Wednesday);
        Assert.IsFalse(entity.Thursday);
        Assert.IsTrue(entity.Friday);
        Assert.IsFalse(entity.Saturday);
        Assert.IsTrue(entity.Sunday);
        Assert.AreEqual(new DateOnly(2026, 1, 1), entity.StartDate);
        Assert.AreEqual(new DateOnly(2026, 6, 30), entity.EndDate);
    }

    [TestMethod]
    public void Map_Calendar_To_GetCalendarDto()
    {
        var calendar = EntityFactory.CreateCalendar(serviceId: 7);

        var dto = Mapper.Map<GetCalendarDto>(calendar);

        Assert.AreEqual(7, dto.ServiceId);
        Assert.IsTrue(dto.Monday);
        Assert.IsFalse(dto.Sunday);
        Assert.AreEqual(calendar.EndDate, dto.EndDate);
    }

    [TestMethod]
    public void Map_CreateIncidentDto_To_Incident()
    {
        var dto = new CreateIncidentDto { IncidentId = 3, RouteId = 9, EstimateDuration = 15 };

        var incident = Mapper.Map<Incident>(dto);

        Assert.AreEqual(3, incident.IncidentId);
        Assert.AreEqual(9, incident.RouteId);
        Assert.AreEqual(15, incident.EstimateDuration);
    }

    [TestMethod]
    public void Map_CreateRouteDto_To_Routes()
    {
        var dto = new CreateRouteDto
        {
            AgencyId = 4,
            RouteShortName = "T1",
            RouteLongName = "Tram 1",
            RouteTypeId = 5
        };

        var route = Mapper.Map<Routes>(dto);

        Assert.AreEqual(4, route.AgencyId);
        Assert.AreEqual("T1", route.RouteShortName);
        Assert.AreEqual("Tram 1", route.RouteLongName);
        Assert.AreEqual(5, route.RouteTypeId);
    }

    [TestMethod]
    public void Map_Routes_To_GetRouteDto()
    {
        var route = EntityFactory.CreateRoute(routeId: 11, agencyId: 2, routeTypeId: 3);

        var dto = Mapper.Map<GetRouteDto>(route);

        Assert.AreEqual(11, dto.RouteId);
        Assert.AreEqual(2, dto.AgencyId);
        Assert.AreEqual("A", dto.RouteShortName);
        Assert.AreEqual("Ligne A", dto.RouteLongName);
    }

    [TestMethod]
    public void Map_UpdateRouteDto_Onto_ExistingRoute()
    {
        var route = EntityFactory.CreateRoute(routeId: 1);
        var dto = new UpdateRouteDto
        {
            RouteShortName = "B",
            RouteLongName = "Ligne B",
            RouteTypeId = 8
        };

        Mapper.Map(dto, route);

        Assert.AreEqual(1, route.RouteId);
        Assert.AreEqual("B", route.RouteShortName);
        Assert.AreEqual("Ligne B", route.RouteLongName);
        Assert.AreEqual(8, route.RouteTypeId);
    }

    [TestMethod]
    public void Map_CreateStopDto_To_StopTimes()
    {
        var dto = new CreateStopDto
        {
            StopName = "Mairie",
            StopLat = 45.5m,
            StopLong = 4.8m,
            AgencyId = 6
        };

        var stop = Mapper.Map<Stop_Times>(dto);

        Assert.AreEqual("Mairie", stop.StopName);
        Assert.AreEqual(45.5m, stop.StopLat);
        Assert.AreEqual(4.8m, stop.StopLong);
        Assert.AreEqual(6, stop.AgencyId);
    }

    [TestMethod]
    public void Map_StopTimes_To_GetStopDto()
    {
        var stop = EntityFactory.CreateStop(stopId: 42);

        var dto = Mapper.Map<GetStopDto>(stop);

        Assert.AreEqual(42, dto.StopId);
        Assert.AreEqual(1, dto.AgencyId);
        Assert.AreEqual("Gare Centrale", dto.StopName);
        Assert.AreEqual(48.85m, dto.StopLat);
        Assert.AreEqual(2.35m, dto.StopLong);
    }

    [TestMethod]
    public void Map_UpdateStopDto_Onto_ExistingStop()
    {
        var stop = EntityFactory.CreateStop(stopId: 5);
        var dto = new UpdateStopDto { StopName = "Nouveau", StopLat = 1m, StopLong = 2m };

        Mapper.Map(dto, stop);

        Assert.AreEqual(5, stop.StopId);
        Assert.AreEqual("Nouveau", stop.StopName);
        Assert.AreEqual(1m, stop.StopLat);
        Assert.AreEqual(2m, stop.StopLong);
    }

    [TestMethod]
    public void Map_CreateStopTripDto_To_StopTrip()
    {
        var dto = new CreateStopTripDto
        {
            TripId = 3,
            StopId = 4,
            StopSequence = 2,
            ArrivalTime = new TimeOnly(10, 0),
            DepartureTime = new TimeOnly(9, 58)
        };

        var stopTrip = Mapper.Map<Stop_Trip>(dto);

        Assert.AreEqual(3, stopTrip.TripId);
        Assert.AreEqual(4, stopTrip.StopId);
        Assert.AreEqual(2, stopTrip.StopSequence);
        Assert.AreEqual(new TimeOnly(10, 0), stopTrip.ArrivalTime);
        Assert.AreEqual(new TimeOnly(9, 58), stopTrip.DepartureTime);
    }

    [TestMethod]
    public void Map_UpdateStopTripDto_Onto_ExistingStopTrip()
    {
        var route = EntityFactory.CreateRoute(1);
        var calendar = EntityFactory.CreateCalendar(1);
        var trip = EntityFactory.CreateTrip(route, calendar, 1);
        var stop = EntityFactory.CreateStop(1);
        var stopTrip = EntityFactory.CreateStopTrip(trip, stop);

        var dto = new UpdateStopTripDto
        {
            ArrivalTime = new TimeOnly(12, 0),
            DepartureTime = new TimeOnly(11, 45)
        };

        Mapper.Map(dto, stopTrip);

        Assert.AreEqual(new TimeOnly(12, 0), stopTrip.ArrivalTime);
        Assert.AreEqual(new TimeOnly(11, 45), stopTrip.DepartureTime);
    }

    [TestMethod]
    public void Map_CreateTripDto_To_Trip()
    {
        var dto = new CreateTripDto
        {
            RouteId = 2,
            ServiceId = 3,
            TripHeadsign = "Terminus Nord"
        };

        var trip = Mapper.Map<Trip>(dto);

        Assert.AreEqual(2, trip.RouteId);
        Assert.AreEqual(3, trip.ServiceId);
        Assert.AreEqual("Terminus Nord", trip.TripHeadsign);
    }

    [TestMethod]
    public void Map_UpdateTripServiceDto_Onto_ExistingTrip()
    {
        var trip = EntityFactory.CreateTrip(EntityFactory.CreateRoute(1), EntityFactory.CreateCalendar(1), 9);
        var dto = new UpdateTripServiceDto { ServiceId = 77 };

        Mapper.Map(dto, trip);

        Assert.AreEqual(77, trip.ServiceId);
        Assert.AreEqual(9, trip.TripId);
    }
}
