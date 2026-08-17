using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using UrbanFlow_trips.API.GrpcServices;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.MsTest.TestHelpers;
using Grpc.Core;

namespace UrbanFlow_trips.MsTest.Integration;

[TestClass]
public class RoutesRepositoryTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();
    private TripsDbContext _context = null!;
    private RoutesRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextFactory.CreateInMemory();
        _repository = CreateRepository("Bus");
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    private RoutesRepository CreateRepository(string vehicleName)
    {
        var vehicleService = new VehicleService(
            GrpcHelpers.CreateVehiclerClientMock(vehicleName).Object,
            NullLogger<VehicleService>.Instance);
        return new RoutesRepository(_context, Mapper, vehicleService);
    }

    private RoutesRepository CreateRepositoryWithUnavailableVehicleService()
    {
        var vehicleService = new VehicleService(
            GrpcHelpers.CreateThrowingVehiclerClientMock(
                new RpcException(new Status(StatusCode.Unavailable, "down"))).Object,
            NullLogger<VehicleService>.Instance);
        return new RoutesRepository(_context, Mapper, vehicleService);
    }

    private async Task<Routes> SeedCompleteRouteAsync(int agencyId = 1, int routeTypeId = 2)
    {
        var route = EntityFactory.CreateRoute(agencyId: agencyId, routeTypeId: routeTypeId);
        var calendar = EntityFactory.CreateCalendar();
        var trip = EntityFactory.CreateTrip(route, calendar);
        var stop1 = EntityFactory.CreateStop(name: "Premier");
        var stop2 = EntityFactory.CreateStop(name: "Second");
        _context.AddRange(route, calendar, trip, stop1, stop2);
        await _context.SaveChangesAsync();

        // Séquences volontairement inversées pour vérifier le tri par StopSequence
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop2, sequence: 2));
        _context.StopTrips.Add(EntityFactory.CreateStopTrip(trip, stop1, sequence: 1));
        await _context.SaveChangesAsync();
        return route;
    }

    [TestMethod]
    public async Task CreateRouteAsync_PersistsRoute()
    {
        await _repository.CreateRouteAsync(new CreateRouteDto
        {
            AgencyId = 1,
            RouteShortName = "A",
            RouteLongName = "Ligne A",
            RouteTypeId = 2
        });

        var route = _context.Routes.Single();
        Assert.AreEqual("A", route.RouteShortName);
        Assert.AreEqual(2, route.RouteTypeId);
    }

    [TestMethod]
    public async Task GetCompleteRouteByIdAsync_ReturnsRouteWithOrderedStops()
    {
        var route = await SeedCompleteRouteAsync();

        var result = await _repository.GetCompleteRouteByIdAsync(route.RouteId);

        Assert.AreEqual(1, result.Count);
        var dto = result[0];
        Assert.AreEqual(route.RouteId, dto.RouteId);
        Assert.AreEqual("Bus", dto.RouteTypeName);
        Assert.AreEqual(1, dto.Trips.Count);
        var stops = dto.Trips[0].Stops.ToList();
        Assert.AreEqual(2, stops.Count);
        Assert.AreEqual("Premier", stops[0].StopName);
        Assert.AreEqual("Second", stops[1].StopName);
        Assert.AreEqual(1, stops[0].SequenceOrder);
        // ArrivalTime exposé en secondes depuis minuit
        Assert.AreEqual(new TimeOnly(8, 0).ToTimeSpan().TotalSeconds, stops[0].ArrivalTime);
    }

    [TestMethod]
    public async Task GetCompleteRouteByIdAsync_UnknownId_ReturnsEmptyList()
    {
        var result = await _repository.GetCompleteRouteByIdAsync(999);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetAllCompleteRoutesAsync_ReturnsAllRoutes()
    {
        await SeedCompleteRouteAsync();

        var result = await _repository.GetAllCompleteRoutesAsync();

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public async Task GetRoutesFilter_FiltersByAgencyAndRouteType()
    {
        await SeedCompleteRouteAsync(agencyId: 1, routeTypeId: 2);
        await SeedCompleteRouteAsync(agencyId: 3, routeTypeId: 4);

        var byAgency = await _repository.GetRoutesFilter(new RouteFilterDto { AgencyId = 3 });
        var byType = await _repository.GetRoutesFilter(new RouteFilterDto { RouteTypeId = 2 });
        var byBoth = await _repository.GetRoutesFilter(new RouteFilterDto { AgencyId = 1, RouteTypeId = 4 });

        Assert.AreEqual(1, byAgency.Count);
        Assert.AreEqual(1, byType.Count);
        Assert.AreEqual(0, byBoth.Count);
    }

    [TestMethod]
    public async Task GetRoutesFilter_NullFilter_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.GetRoutesFilter(null!));
    }

    [TestMethod]
    public async Task GetRoutesFilter_VehicleServiceUnavailable_UsesNullFallback()
    {
        var route = await SeedCompleteRouteAsync();
        var repository = CreateRepositoryWithUnavailableVehicleService();

        var result = await repository.GetCompleteRouteByIdAsync(route.RouteId);

        Assert.AreEqual("null", result[0].RouteTypeName);
    }

    [TestMethod]
    public async Task UpdateRouteAsync_ExistingRoute_UpdatesValues()
    {
        var route = EntityFactory.CreateRoute();
        _context.Routes.Add(route);
        await _context.SaveChangesAsync();

        await _repository.UpdateRouteAsync(route.RouteId,
            new UpdateRouteDto { RouteShortName = "Z", RouteLongName = "Ligne Z", RouteTypeId = 7 });

        var updated = _context.Routes.Single();
        Assert.AreEqual("Z", updated.RouteShortName);
        Assert.AreEqual(7, updated.RouteTypeId);
    }

    [TestMethod]
    public async Task UpdateRouteAsync_UnknownRoute_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.UpdateRouteAsync(999,
                new UpdateRouteDto { RouteShortName = "X", RouteLongName = "X", RouteTypeId = 1 }));
    }

    [TestMethod]
    public async Task DeleteRouteAsync_ExistingRoute_RemovesIt()
    {
        var route = EntityFactory.CreateRoute();
        _context.Routes.Add(route);
        await _context.SaveChangesAsync();

        await _repository.DeleteRouteAsync(route.RouteId);

        Assert.AreEqual(0, _context.Routes.Count());
    }

    [TestMethod]
    public async Task DeleteRouteAsync_UnknownRoute_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.DeleteRouteAsync(999));
    }

    [TestMethod]
    public async Task GetAllRoutesAsync_ReturnsRoutesWithVehicleName()
    {
        _context.Routes.Add(EntityFactory.CreateRoute());
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllRoutesAsync();

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Bus", result[0].RouteTypeName);
        Assert.AreEqual("A", result[0].RouteShortName);
    }

    [TestMethod]
    public async Task GetAllRoutesAsync_EmptyDb_ReturnsEmptyList()
    {
        var result = await _repository.GetAllRoutesAsync();
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task RouteExistsAsync_ReturnsTrueWhenExists_FalseOtherwise()
    {
        var route = EntityFactory.CreateRoute();
        _context.Routes.Add(route);
        await _context.SaveChangesAsync();

        Assert.IsTrue(await _repository.RouteExistsAsync(route.RouteId));
        Assert.IsFalse(await _repository.RouteExistsAsync(999));
    }
}
