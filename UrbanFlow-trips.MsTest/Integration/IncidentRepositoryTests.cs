using AutoMapper;
using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Integration;

[TestClass]
public class IncidentRepositoryTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();
    private TripsDbContext _context = null!;
    private IncidentRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextFactory.CreateInMemory();
        _repository = new IncidentRepository(_context, Mapper);
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    [TestMethod]
    public async Task CreateIncidentAsync_PersistsIncident()
    {
        var dto = new CreateIncidentDto { IncidentId = 5, RouteId = 2, EstimateDuration = 12 };

        await _repository.CreateIncidentAsync(dto);

        var incident = _context.Incidents.Single();
        Assert.AreEqual(5, incident.IncidentId);
        Assert.AreEqual(2, incident.RouteId);
        Assert.AreEqual(12, incident.EstimateDuration);
    }

    [TestMethod]
    public async Task CreateIncidentAsync_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.CreateIncidentAsync(null!));
    }

    [TestMethod]
    public async Task DeleteIncidentAsync_ExistingIncident_RemovesAndReturnsIt()
    {
        var incident = new Incident { IncidentId = 7, RouteId = 1, EstimateDuration = 5 };
        _context.Incidents.Add(incident);
        await _context.SaveChangesAsync();

        var deleted = await _repository.DeleteIncidentAsync(incident.Id);

        Assert.IsNotNull(deleted);
        Assert.AreEqual(7, deleted.IncidentId);
        Assert.AreEqual(0, _context.Incidents.Count());
    }

    [TestMethod]
    public async Task DeleteIncidentAsync_UnknownId_ReturnsNull()
    {
        Assert.IsNull(await _repository.DeleteIncidentAsync(12345));
    }

    [TestMethod]
    public async Task EstimateMinutesLateByRouteId_PositiveDuration_ReturnsIt()
    {
        _context.Incidents.Add(new Incident { IncidentId = 1, RouteId = 9, EstimateDuration = 25 });
        await _context.SaveChangesAsync();

        Assert.AreEqual(25, await _repository.EstimateMinutesLateByRouteId(9));
    }

    [TestMethod]
    public async Task EstimateMinutesLateByRouteId_ZeroDuration_ReturnsNull()
    {
        _context.Incidents.Add(new Incident { IncidentId = 1, RouteId = 9, EstimateDuration = 0 });
        await _context.SaveChangesAsync();

        Assert.IsNull(await _repository.EstimateMinutesLateByRouteId(9));
    }

    [TestMethod]
    public async Task EstimateMinutesLateByRouteId_NoIncident_ReturnsNull()
    {
        Assert.IsNull(await _repository.EstimateMinutesLateByRouteId(42));
    }

    [TestMethod]
    public async Task IsRouteIdImpacted_ReflectsIncidentPresence()
    {
        _context.Incidents.Add(new Incident { IncidentId = 1, RouteId = 3, EstimateDuration = 2 });
        await _context.SaveChangesAsync();

        Assert.IsTrue(_repository.IsRouteIdImpacted(3));
        Assert.IsFalse(_repository.IsRouteIdImpacted(4));
    }
}
