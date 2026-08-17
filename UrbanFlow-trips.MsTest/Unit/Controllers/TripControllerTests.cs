using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanFlow_trips.API.Controllers;
using UrbanFlow_trips.Application.DTO.StopTrip;
using UrbanFlow_trips.Application.DTO.Trip;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.Controllers;

[TestClass]
public class TripControllerTests
{
    private readonly Mock<ITripRepository> _tripRepo = new();
    private readonly Mock<IStopTripRepository> _stopTripRepo = new();
    private TripController CreateController() => new(_tripRepo.Object, _stopTripRepo.Object);

    [TestMethod]
    public async Task CreateTrip_ReturnsOk_AndCallsRepository()
    {
        var dto = new CreateTripDto { RouteId = 1, ServiceId = 1, TripHeadsign = "Centre" };

        var result = await CreateController().CreateTrip(dto);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _tripRepo.Verify(r => r.CreateTripAsync(dto), Times.Once);
    }

    [TestMethod]
    public async Task UpdateHourlyTrip_ReturnsOk_AndCallsRepository()
    {
        var dto = new UpdateStopTripDto { ArrivalTime = new TimeOnly(9, 0) };

        var result = await CreateController().UpdateHourlyTrip(dto, 2, 3);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _stopTripRepo.Verify(r => r.UpdateStopTripAsync(2, 3, dto), Times.Once);
    }

    [TestMethod]
    public async Task UpdateTripService_ReturnsOk_AndCallsRepository()
    {
        var dto = new UpdateTripServiceDto { ServiceId = 5 };

        var result = await CreateController().UpdateTripService(dto, 7);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _tripRepo.Verify(r => r.UpdateTripService(7, dto), Times.Once);
    }

    [TestMethod]
    public async Task DeleteStopTrip_ReturnsOk_AndCallsRepository()
    {
        var result = await CreateController().DeleteStopTrip(4, 5);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _stopTripRepo.Verify(r => r.DeleteStopTripAsync(4, 5), Times.Once);
    }

    [TestMethod]
    public async Task DeleteTrip_ReturnsOk_AndCallsRepository()
    {
        var result = await CreateController().DeleteTrip(9);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _tripRepo.Verify(r => r.DeleteTripAsync(9), Times.Once);
    }
}
