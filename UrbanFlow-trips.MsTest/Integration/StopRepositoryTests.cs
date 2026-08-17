using AutoMapper;
using UrbanFlow_trips.Application.DTO.Stop;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Integration;

[TestClass]
public class StopRepositoryTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();
    private TripsDbContext _context = null!;
    private StopRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextFactory.CreateInMemory();
        _repository = new StopRepository(_context, Mapper);
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    [TestMethod]
    public async Task CreateStopAsync_PersistsStop()
    {
        var dto = new CreateStopDto { StopName = "Gare", StopLat = 48m, StopLong = 2m, AgencyId = 1 };

        await _repository.CreateStopAsync(dto);

        var stop = _context.Stops.Single();
        Assert.AreEqual("Gare", stop.StopName);
        Assert.AreEqual(48m, stop.StopLat);
    }

    [TestMethod]
    public async Task CreateStopAsync_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.CreateStopAsync(null!));
    }

    [TestMethod]
    public async Task UpdateStopAsync_ExistingStop_UpdatesValues()
    {
        var stop = EntityFactory.CreateStop();
        _context.Stops.Add(stop);
        await _context.SaveChangesAsync();

        await _repository.UpdateStopAsync(stop.StopId,
            new UpdateStopDto { StopName = "Renommé", StopLat = 1m, StopLong = 2m });

        var updated = _context.Stops.Single();
        Assert.AreEqual("Renommé", updated.StopName);
        Assert.AreEqual(1m, updated.StopLat);
        Assert.AreEqual(2m, updated.StopLong);
    }

    [TestMethod]
    public async Task UpdateStopAsync_UnknownId_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.UpdateStopAsync(999,
                new UpdateStopDto { StopName = "X", StopLat = 0m, StopLong = 0m }));
    }

    [TestMethod]
    public async Task UpdateStopAsync_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.UpdateStopAsync(1, null!));
    }

    [TestMethod]
    public async Task DeleteStopAsync_ExistingStop_RemovesIt()
    {
        var stop = EntityFactory.CreateStop();
        _context.Stops.Add(stop);
        await _context.SaveChangesAsync();

        await _repository.DeleteStopAsync(stop.StopId);

        Assert.AreEqual(0, _context.Stops.Count());
    }

    [TestMethod]
    public async Task DeleteStopAsync_UnknownId_ThrowsKeyNotFound()
    {
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _repository.DeleteStopAsync(999));
    }

    [TestMethod]
    public async Task GetAllStopsAsync_ReturnsMappedDtos()
    {
        _context.Stops.Add(EntityFactory.CreateStop(name: "S1"));
        _context.Stops.Add(EntityFactory.CreateStop(name: "S2"));
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllStopsAsync();

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEquivalent(new[] { "S1", "S2" }, result.Select(s => s.StopName).ToArray());
    }

    [TestMethod]
    public async Task StopExistsAsync_ReturnsTrueWhenExists_FalseOtherwise()
    {
        var stop = EntityFactory.CreateStop();
        _context.Stops.Add(stop);
        await _context.SaveChangesAsync();

        Assert.IsTrue(await _repository.StopExistsAsync(stop.StopId));
        Assert.IsFalse(await _repository.StopExistsAsync(888));
    }
}
