using Microsoft.AspNetCore.Mvc;
using Moq;
using UrbanFlow_trips.API.Controllers;
using UrbanFlow_trips.Application.DTO.CompleteRoute;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.Controllers;

[TestClass]
public class RoutesControllerTests
{
    private readonly Mock<IRoutesRepository> _repo = new();
    private readonly Mock<IGetAdjustedRoutesUseCase> _useCase = new();
    private RoutesController CreateController() => new(_repo.Object, _useCase.Object);

    private static List<GetCompleteRouteDto> CompleteRoutes() =>
    [
        new() { RouteId = 1, RouteShortName = "A", RouteLongName = "Ligne A", RouteTypeName = "Bus" }
    ];

    [TestMethod]
    public async Task CreateAgency_ReturnsOk_AndCallsRepository()
    {
        var dto = new CreateRouteDto { AgencyId = 1, RouteShortName = "A", RouteLongName = "Ligne A", RouteTypeId = 2 };

        var result = await CreateController().CreateAgency(dto);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _repo.Verify(r => r.CreateRouteAsync(dto), Times.Once);
    }

    [TestMethod]
    public async Task GetAllRoutes_ReturnsOkWithRoutes()
    {
        var routes = new List<GetRouteDto> { new() { RouteId = 1 } };
        _repo.Setup(r => r.GetAllRoutesAsync()).ReturnsAsync(routes);

        var result = await CreateController().GetAllRoutes();

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreSame(routes, ok.Value);
    }

    [TestMethod]
    public async Task GetRouteDetails_ReturnsOkWithRoute()
    {
        var routes = CompleteRoutes();
        _repo.Setup(r => r.GetCompleteRouteByIdAsync(1)).ReturnsAsync(routes);

        var result = await CreateController().GetRouteDetails(1);

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreSame(routes, ok.Value);
    }

    [TestMethod]
    public async Task GetAllCompleteRoutes_ReturnsOk()
    {
        var routes = CompleteRoutes();
        _repo.Setup(r => r.GetAllCompleteRoutesAsync()).ReturnsAsync(routes);

        var result = await CreateController().GetAllCompleteRoutes();

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreSame(routes, ok.Value);
    }

    [TestMethod]
    public async Task GetRoutes_UsesUseCase()
    {
        var filter = new RouteFilterDto { AgencyId = 1 };
        var routes = CompleteRoutes();
        _useCase.Setup(u => u.ExecuteAsync(filter)).ReturnsAsync(routes);

        var result = await CreateController().GetRoutes(filter);

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreSame(routes, ok.Value);
        _useCase.Verify(u => u.ExecuteAsync(filter), Times.Once);
    }

    [TestMethod]
    public async Task UpdateRoute_ReturnsOk_AndCallsRepository()
    {
        var dto = new UpdateRouteDto { RouteShortName = "B", RouteLongName = "Ligne B", RouteTypeId = 1 };

        var result = await CreateController().UpdateRoute(dto, 4);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _repo.Verify(r => r.UpdateRouteAsync(4, dto), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAgency_ReturnsOk_AndCallsRepository()
    {
        var result = await CreateController().DeleteAgency(6);

        Assert.IsInstanceOfType<OkObjectResult>(result);
        _repo.Verify(r => r.DeleteRouteAsync(6), Times.Once);
    }
}
