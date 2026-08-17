using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UrbanFlow_trips.API.GrpcServices;
using UrbanFlow_trips.Application.DTO.StopTrip;
using UrbanFlow_trips.Application.DTO.Trip;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.Infrastructure.Exceptions;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Integration;

[TestClass]
public class TripRepositoryTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();
    private TripsDbContext _context = null!;
    private StopRepository _stopRepository = null!;
    private CalendarRepository _calendarRepository = null!;
    private RoutesRepository _routesRepository = null!;
    private StopTripRepository _stopTripRepository = null!;
    private TripRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextFactory.CreateInMemory();
        _stopRepository = new StopRepository(_context, Mapper);
        _calendarRepository = new CalendarRepository(_context, Mapper);
        var vehicleService = new VehicleService(
            GrpcHelpers.CreateVehiclerClientMock("Bus").Object,
            NullLogger<VehicleService>.Instance);
        _routesRepository = new RoutesRepository(_context, Mapper, vehicleService);
        _stopTripRepository = new StopTripRepository(_context, Mapper, _stopRepository);
        _repository = new TripRepository(_context, _stopTripRepository, Mapper,
            _routesRepository, _calendarRepository, _stopRepository);
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    private async Task<(Routes route, Calendar calendar, Stop_Times stop)> SeedAsync()
    {
        var route = EntityFactory.CreateRoute();
        var calendar = EntityFactory.CreateCalendar();
        var stop = EntityFactory.CreateStop();
        _context.AddRange(route, calendar, stop);
        await _context.SaveChangesAsync();
        return (route, calendar, stop);
    }

    [TestMethod]
    public async Task CreateTripAsync_ValidDto_CreatesTripAndStopTrips()
    {
        var (route, calendar, stop) = await SeedAsync();
        var dto = new CreateTripDto
        {
            RouteId = route.RouteId,
            ServiceId = calendar.ServiceId,
            TripHeadsign = "Direction Centre",
            CreateStopTrips =
            [
                new CreateStopTripDto { StopId = stop.StopId, ArrivalTime = new TimeOnly(8, 0), DepartureTime = null }
            ]
        };

        await _repository.CreateTripAsync(dto);

        var trip = _context.Trips.Single();
        Assert.AreEqual("Direction Centre", trip.TripHeadsign);
        var stopTrip = _context.StopTrips.Single();
        Assert.AreEqual(trip.TripId, stopTrip.TripId);
        Assert.AreEqual(1, stopTrip.StopSequence);
        // DepartureTime null est remplacé par ArrivalTime
        Assert.AreEqual(new TimeOnly(8, 0), stopTrip.DepartureTime);
    }

    [TestMethod]
    public async Task CreateTripAsync_KeepsProvidedDepartureTime_AndIncrementsSequence()
    {
        var (route, calendar, stop) = await SeedAsync();
        var stop2 = EntityFactory.CreateStop(name: "Second");
        _context.Stops.Add(stop2);
        await _context.SaveChangesAsync();

        var dto = new CreateTripDto
        {
            RouteId = route.RouteId,
            ServiceId = calendar.ServiceId,
            TripHeadsign = "Multi-arrêts",
            CreateStopTrips =
            [
                new CreateStopTripDto { StopId = stop.StopId, ArrivalTime = new TimeOnly(8, 0), DepartureTime = new TimeOnly(7, 55) },
                new CreateStopTripDto { StopId = stop2.StopId, ArrivalTime = new TimeOnly(8, 10), DepartureTime = null }
            ]
        };

        await _repository.CreateTripAsync(dto);

        var stopTrips = _context.StopTrips.OrderBy(st => st.StopSequence).ToList();
        Assert.AreEqual(2, stopTrips.Count);
        Assert.AreEqual(new TimeOnly(7, 55), stopTrips[0].DepartureTime);
        Assert.AreEqual(2, stopTrips[1].StopSequence);
    }

    [TestMethod]
    public async Task CreateTripAsync_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.CreateTripAsync(null!));
    }

    [TestMethod]
    public async Task CreateTripAsync_UnknownRoute_ThrowsNotFound()
    {
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _repository.CreateTripAsync(new CreateTripDto { RouteId = 999, ServiceId = 1 }));
        Assert.AreEqual(404, ex.ErrorCode);
    }

    [TestMethod]
    public async Task CreateTripAsync_UnknownCalendar_ThrowsNotFound()
    {
        var (route, _, _) = await SeedAsync();

        await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _repository.CreateTripAsync(new CreateTripDto { RouteId = route.RouteId, ServiceId = 999 }));
    }

    [TestMethod]
    public async Task CreateTripAsync_UnknownStop_ThrowsNotFound()
    {
        var (route, calendar, _) = await SeedAsync();
        var dto = new CreateTripDto
        {
            RouteId = route.RouteId,
            ServiceId = calendar.ServiceId,
            CreateStopTrips = [new CreateStopTripDto { StopId = 999, ArrivalTime = new TimeOnly(8, 0) }]
        };

        await Assert.ThrowsExceptionAsync<NotFoundException>(() => _repository.CreateTripAsync(dto));
    }

    [TestMethod]
    public async Task CreateTripAsync_StopTripFailure_RollsBackAndWrapsException()
    {
        var (route, calendar, stop) = await SeedAsync();
        var failingStopTripRepo = new Mock<IStopTripRepository>();
        failingStopTripRepo
            .Setup(r => r.CreateStopTripAsync(It.IsAny<CreateStopTripDto>()))
            .ThrowsAsync(new InvalidOperationException("boom"));
        var repository = new TripRepository(_context, failingStopTripRepo.Object, Mapper,
            _routesRepository, _calendarRepository, _stopRepository);

        var dto = new CreateTripDto
        {
            RouteId = route.RouteId,
            ServiceId = calendar.ServiceId,
            TripHeadsign = "Echec",
            CreateStopTrips = [new CreateStopTripDto { StopId = stop.StopId, ArrivalTime = new TimeOnly(8, 0) }]
        };

        var ex = await Assert.ThrowsExceptionAsync<Exception>(() => repository.CreateTripAsync(dto));
        StringAssert.StartsWith(ex.Message, "Error creating trip");
    }

    [TestMethod]
    public async Task UpdateTripService_ExistingTrip_UpdatesServiceId()
    {
        var (route, calendar, _) = await SeedAsync();
        var calendar2 = EntityFactory.CreateCalendar();
        var trip = EntityFactory.CreateTrip(route, calendar);
        _context.AddRange(calendar2, trip);
        await _context.SaveChangesAsync();

        await _repository.UpdateTripService(trip.TripId, new UpdateTripServiceDto { ServiceId = calendar2.ServiceId });

        Assert.AreEqual(calendar2.ServiceId, _context.Trips.Single().ServiceId);
    }

    [TestMethod]
    public async Task UpdateTripService_UnknownTrip_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.UpdateTripService(999, new UpdateTripServiceDto { ServiceId = 1 }));
    }

    [TestMethod]
    public async Task UpdateTripService_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.UpdateTripService(1, null!));
    }

    [TestMethod]
    public async Task DeleteTripAsync_ExistingTrip_RemovesIt()
    {
        var (route, calendar, _) = await SeedAsync();
        var trip = EntityFactory.CreateTrip(route, calendar);
        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        await _repository.DeleteTripAsync(trip.TripId);

        Assert.AreEqual(0, _context.Trips.Count());
    }

    [TestMethod]
    public async Task DeleteTripAsync_UnknownTrip_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.DeleteTripAsync(999));
    }

    [TestMethod]
    public async Task GetAllTripsAsync_ReturnsAllTrips()
    {
        var (route, calendar, _) = await SeedAsync();
        _context.Trips.Add(EntityFactory.CreateTrip(route, calendar));
        await _context.SaveChangesAsync();

        var trips = await _repository.GetAllTripsAsync();

        Assert.AreEqual(1, trips.Count);
    }

    [TestMethod]
    public async Task TripExistsAsync_ReturnsTrueWhenExists_FalseOtherwise()
    {
        var (route, calendar, _) = await SeedAsync();
        var trip = EntityFactory.CreateTrip(route, calendar);
        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        Assert.IsTrue(await _repository.TripExistsAsync(trip.TripId));
        Assert.IsFalse(await _repository.TripExistsAsync(999));
    }
}
