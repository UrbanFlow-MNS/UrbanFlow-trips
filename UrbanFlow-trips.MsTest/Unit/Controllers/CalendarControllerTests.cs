using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanFlow_trips.API.Controllers;
using UrbanFlow_trips.Application.DTO.Calendar;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.Controllers;

[TestClass]
public class CalendarControllerTests
{
    [TestMethod]
    public async Task CreateCalendar_ReturnsOk_AndCallsRepository()
    {
        var repo = new Mock<ICalendarRepository>();
        var controller = new CalendarController(repo.Object);
        var dto = new CreateCalendarDto { Monday = true };

        var result = await controller.CreateCalendar(dto);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        repo.Verify(r => r.CreateCalendarAsync(dto), Times.Once);
    }

    [TestMethod]
    public async Task GetAllCalendars_ReturnsOkWithCalendars()
    {
        var calendars = new List<GetCalendarDto> { new() { ServiceId = 1 } };
        var repo = new Mock<ICalendarRepository>();
        repo.Setup(r => r.GetAllCalendarsAsync()).ReturnsAsync(calendars);
        var controller = new CalendarController(repo.Object);

        var result = await controller.GetAllCalendars();

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreSame(calendars, ok.Value);
    }
}
