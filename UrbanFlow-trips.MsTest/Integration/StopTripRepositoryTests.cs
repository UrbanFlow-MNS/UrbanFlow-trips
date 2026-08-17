using AutoMapper;
using UrbanFlow_trips.Application.DTO.StopTrip;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.Infrastructure.Exceptions;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Integration;

[TestClass]
public class StopTripRepositoryTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();
    private TripsDbContext _context = null!;
    private StopTripRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextFactory.CreateInMemory();
        _repository = new StopTripRepository(_context, Mapper, new StopRepository(_context, Mapper));
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    private async Task<(Trip trip, Stop_Times stop)> SeedTripAndStopAsync()
    {
        var route = EntityFactory.CreateRoute();
        var calendar = EntityFactory.CreateCalendar();
        var trip = EntityFactory.CreateTrip(route, calendar);
        var stop = EntityFactory.CreateStop();
        _context.AddRange(route, calendar, trip, stop);
        await _context.SaveChangesAsync();
        return (trip, stop);
    }

    [TestMethod]
    public async Task CreateStopTripAsync_ExistingStop_AddsStopTrip()
    {
        var (trip, stop) = await SeedTripAndStopAsync();
        var dto = new CreateStopTripDto
        {
            TripId = trip.TripId,
            StopId = stop.StopId,
            StopSequence = 1,
            ArrivalTime = new TimeOnly(8, 0),
            DepartureTime = new TimeOnly(7, 55)
        };

        await _repository.CreateStopTripAsync(dto);
        await _context.SaveChangesAsync();

        var stopTrip = _context.StopTrips.Single();
        Assert.AreEqual(trip.TripId, stopTrip.TripId);
        Assert.AreEqual(stop.StopId, stopTrip.StopId);
    }

    [TestMethod]
    public async Task CreateStopTripAsync_UnknownStop_ThrowsNotFound()
    {
        var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _repository.CreateStopTripAsync(new CreateStopTripDto { StopId = 999 }));
        Assert.AreEqual(404, ex.ErrorCode);
    }

    [TestMethod]
    public async Task CreateStopTripAsync_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.CreateStopTripAsync(null!));
    }

    [TestMethod]
    public async Task GetStopTripByIdAsync_ReturnsEntityOrNull()
    {
        var (trip, stop) = await SeedTripAndStopAsync();
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop));
        await _context.SaveChangesAsync();

        Assert.IsNotNull(await _repository.GetStopTripByIdAsync(stop.StopId, trip.TripId));
        Assert.IsNull(await _repository.GetStopTripByIdAsync(999, 999));
    }

    [TestMethod]
    public async Task GetAllStopTripsAsync_ReturnsAll()
    {
        var (trip, stop) = await SeedTripAndStopAsync();
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop));
        await _context.SaveChangesAsync();

        var all = await _repository.GetAllStopTripsAsync();

        Assert.AreEqual(1, all.Count);
    }

    [TestMethod]
    public async Task UpdateStopTripAsync_ExistingEntity_UpdatesTimes()
    {
        var (trip, stop) = await SeedTripAndStopAsync();
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop));
        await _context.SaveChangesAsync();

        await _repository.UpdateStopTripAsync(stop.StopId, trip.TripId,
            new UpdateStopTripDto { ArrivalTime = new TimeOnly(10, 30), DepartureTime = new TimeOnly(10, 15) });

        var updated = _context.StopTrips.Single();
        Assert.AreEqual(new TimeOnly(10, 30), updated.ArrivalTime);
        Assert.AreEqual(new TimeOnly(10, 15), updated.DepartureTime);
    }

    [TestMethod]
    public async Task UpdateStopTripAsync_UnknownEntity_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.UpdateStopTripAsync(1, 1,
                new UpdateStopTripDto { ArrivalTime = new TimeOnly(10, 0) }));
    }

    [TestMethod]
    public async Task DeleteStopTripAsync_ExistingEntity_RemovesIt()
    {
        var (trip, stop) = await SeedTripAndStopAsync();
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop));
        await _context.SaveChangesAsync();

        await _repository.DeleteStopTripAsync(stop.StopId, trip.TripId);

        Assert.AreEqual(0, _context.StopTrips.Count());
    }

    [TestMethod]
    public async Task DeleteStopTripAsync_UnknownEntity_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.DeleteStopTripAsync(1, 1));
    }

    [TestMethod]
    public async Task StopTripExistsAsync_ReturnsTrueWhenExists_FalseOtherwise()
    {
        var (trip, stop) = await SeedTripAndStopAsync();
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop));
        await _context.SaveChangesAsync();

        Assert.IsTrue(await _repository.StopTripExistsAsync(stop.StopId, trip.TripId));
        Assert.IsFalse(await _repository.StopTripExistsAsync(999, 999));
    }
}
