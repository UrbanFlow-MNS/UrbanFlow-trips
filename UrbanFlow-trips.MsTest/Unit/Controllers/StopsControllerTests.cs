using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanFlow_trips.API.Controllers;
using UrbanFlow_trips.Application.DTO.Stop;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.Controllers;

[TestClass]
public class StopsControllerTests
{
    private readonly Mock<IStopRepository> _repo = new();
    private StopsController CreateController() => new(_repo.Object);

    [TestMethod]
    public async Task CreateStop_ReturnsOk_AndCallsRepository()
    {
        var dto = new CreateStopDto { StopName = "Gare", StopLat = 1m, StopLong = 2m };

        var result = await CreateController().CreateStop(dto);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _repo.Verify(r => r.CreateStopAsync(dto), Times.Once);
    }

    [TestMethod]
    public async Task GetAllStops_ReturnsOkWithStops()
    {
        var stops = new List<GetStopDto> { new() { StopId = 1 } };
        _repo.Setup(r => r.GetAllStopsAsync()).ReturnsAsync(stops);

        var result = await CreateController().GetAllStops();

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreSame(stops, ok.Value);
    }

    [TestMethod]
    public async Task UpdateStop_ReturnsOk_AndCallsRepository()
    {
        var dto = new UpdateStopDto { StopName = "Mairie", StopLat = 1m, StopLong = 2m };

        var result = await CreateController().UpdateStop(dto, 3);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _repo.Verify(r => r.UpdateStopAsync(3, dto), Times.Once);
    }

    [TestMethod]
    public async Task DeleteStop_ReturnsOk_AndCallsRepository()
    {
        var result = await CreateController().DeleteStop(8);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _repo.Verify(r => r.DeleteStopAsync(8), Times.Once);
    }
}
