using Moq;
using UrbanFlow_trips.Application.DTO.CompleteRoute;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Application.UseCases;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.UseCases;

[TestClass]
public class GetAdjustedRoutesUseCaseTests
{
    private static GetCompleteRouteDto BuildRoute(int routeId, double arrivalTime)
        => new()
        {
            RouteId = routeId,
            RouteShortName = "A",
            RouteLongName = "Ligne A",
            RouteTypeName = "Bus",
            Trips =
            [
                new GetTripDetailsDto
                {
                    TripId = 1,
                    Stops =
                    [
                        new GetStopDetailsDto
                        {
                            StopId = 1,
                            StopName = "Gare",
                            Latitude = 48m,
                            Longitude = 2m,
                            ArrivalTime = arrivalTime,
                            SequenceOrder = 1
                        }
                    ]
                }
            ]
        };

    [TestMethod]
    public async Task ExecuteAsync_WithDelay_AddsDelayInSecondsToArrivalTimes()
    {
        var routes = new List<GetCompleteRouteDto> { BuildRoute(1, 3600) };
        var routesRepo = new Mock<IRoutesRepository>();
        routesRepo.Setup(r => r.GetRoutesFilter(It.IsAny<RouteFilterDto>())).ReturnsAsync(routes);
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(r => r.EstimateMinutesLateByRouteId(1)).ReturnsAsync(10);

        var useCase = new GetAdjustedRoutesUseCase(routesRepo.Object, incidentRepo.Object);
        var result = await useCase.ExecuteAsync(new RouteFilterDto());

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(3600 + 600, result[0].Trips[0].Stops.First().ArrivalTime);
    }

    [TestMethod]
    public async Task ExecuteAsync_NoDelay_LeavesArrivalTimesUnchanged()
    {
        var routes = new List<GetCompleteRouteDto> { BuildRoute(2, 1200) };
        var routesRepo = new Mock<IRoutesRepository>();
        routesRepo.Setup(r => r.GetRoutesFilter(It.IsAny<RouteFilterDto>())).ReturnsAsync(routes);
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(r => r.EstimateMinutesLateByRouteId(2)).ReturnsAsync((int?)null);

        var useCase = new GetAdjustedRoutesUseCase(routesRepo.Object, incidentRepo.Object);
        var result = await useCase.ExecuteAsync(new RouteFilterDto { RouteId = 2 });

        Assert.AreEqual(1200, result[0].Trips[0].Stops.First().ArrivalTime);
    }

    [TestMethod]
    public async Task ExecuteAsync_MixedRoutes_OnlyDelayedRoutesAreAdjusted()
    {
        var delayed = BuildRoute(1, 100);
        var onTime = BuildRoute(2, 200);
        var routesRepo = new Mock<IRoutesRepository>();
        routesRepo.Setup(r => r.GetRoutesFilter(It.IsAny<RouteFilterDto>()))
            .ReturnsAsync([delayed, onTime]);
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(r => r.EstimateMinutesLateByRouteId(1)).ReturnsAsync(2);
        incidentRepo.Setup(r => r.EstimateMinutesLateByRouteId(2)).ReturnsAsync((int?)null);

        var useCase = new GetAdjustedRoutesUseCase(routesRepo.Object, incidentRepo.Object);
        var result = await useCase.ExecuteAsync(new RouteFilterDto());

        Assert.AreEqual(220, result[0].Trips[0].Stops.First().ArrivalTime);
        Assert.AreEqual(200, result[1].Trips[0].Stops.First().ArrivalTime);
    }

    [TestMethod]
    public async Task ExecuteAsync_EmptyRoutes_ReturnsEmptyList()
    {
        var routesRepo = new Mock<IRoutesRepository>();
        routesRepo.Setup(r => r.GetRoutesFilter(It.IsAny<RouteFilterDto>()))
            .ReturnsAsync([]);
        var incidentRepo = new Mock<IIncidentRepository>(MockBehavior.Strict);

        var useCase = new GetAdjustedRoutesUseCase(routesRepo.Object, incidentRepo.Object);
        var result = await useCase.ExecuteAsync(new RouteFilterDto());

        Assert.AreEqual(0, result.Count);
    }
}
