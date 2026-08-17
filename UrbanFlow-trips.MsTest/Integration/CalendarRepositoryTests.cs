using AutoMapper;
using UrbanFlow_trips.Application.DTO.Calendar;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Integration;

[TestClass]
public class CalendarRepositoryTests
{
    private static readonly IMapper Mapper = MapperFactory.Create();
    private TripsDbContext _context = null!;
    private CalendarRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextFactory.CreateInMemory();
        _repository = new CalendarRepository(_context, Mapper);
    }

    [TestCleanup]
    public void Cleanup() => _context.Dispose();

    [TestMethod]
    public async Task CreateCalendarAsync_PersistsCalendar()
    {
        var dto = new CreateCalendarDto
        {
            Monday = true,
            Friday = true,
            StartDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 12, 31)
        };

        await _repository.CreateCalendarAsync(dto);

        var calendar = _context.Calendars.Single();
        Assert.IsTrue(calendar.Monday);
        Assert.IsFalse(calendar.Tuesday);
        Assert.AreEqual(new DateOnly(2026, 12, 31), calendar.EndDate);
    }

    [TestMethod]
    public async Task CreateCalendarAsync_NullDto_Throws()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => _repository.CreateCalendarAsync(null!));
    }

    [TestMethod]
    public async Task GetAllCalendarsAsync_ReturnsMappedDtos()
    {
        _context.Calendars.Add(EntityFactory.CreateCalendar());
        _context.Calendars.Add(EntityFactory.CreateCalendar());
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllCalendarsAsync();

        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(c => c.ServiceId > 0));
    }

    [TestMethod]
    public async Task GetAllCalendarsAsync_EmptyDb_ReturnsEmptyList()
    {
        var result = await _repository.GetAllCalendarsAsync();
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task CalendarExistsAsync_ReturnsTrueWhenExists_FalseOtherwise()
    {
        var calendar = EntityFactory.CreateCalendar();
        _context.Calendars.Add(calendar);
        await _context.SaveChangesAsync();

        Assert.IsTrue(await _repository.CalendarExistsAsync(calendar.ServiceId));
        Assert.IsFalse(await _repository.CalendarExistsAsync(9999));
    }
}
