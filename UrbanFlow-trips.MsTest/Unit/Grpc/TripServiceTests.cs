using Moq;
using UrbanFlow_trips.API.GrpcServices;
using UrbanFlow_trips.Application.DTO.CompleteRoute;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Unit.Grpc;

[TestClass]
public class TripServiceTests
{
    private static List<GetCompleteRouteDto> SampleRoutes() =>
    [
        new()
        {
            RouteId = 1,
            RouteShortName = "A",
            RouteLongName = "Ligne A",
            RouteTypeName = "Bus",
            Trips =
            [
                new GetTripDetailsDto
                {
                    TripId = 10,
                    Stops =
                    [
                        new GetStopDetailsDto
                        {
                            StopId = 100,
                            StopName = "Gare",
                            Longitude = 2.35m,
                            Latitude = 48.85m,
                            ArrivalTime = 28800,
                            SequenceOrder = 1
                        }
                    ]
                }
            ]
        }
    ];

    [TestMethod]
    public async Task FindAll_MapsRoutesToGrpcResponse()
    {
        var useCase = new Mock<IGetAdjustedRoutesUseCase>();
        useCase.Setup(u => u.ExecuteAsync(It.Is<RouteFilterDto>(f => f.RouteId == null)))
            .ReturnsAsync(SampleRoutes());
        var service = new TripService(useCase.Object);

        var response = await service.FindAll(new Empty(), new FakeServerCallContext());

        Assert.AreEqual(1, response.Routes.Count);
        var route = response.Routes[0];
        Assert.AreEqual(1, route.RouteId);
        Assert.AreEqual("A", route.RouteShortName);
        Assert.AreEqual("Ligne A", route.RouteLongName);
        Assert.AreEqual("Bus", route.RouteTypeName);
        Assert.AreEqual(1, route.Trips.Count);
        Assert.AreEqual(10, route.Trips[0].TripId);
        var stop = route.Trips[0].Stops[0];
        Assert.AreEqual(100, stop.StopId);
        Assert.AreEqual("Gare", stop.StopName);
        Assert.AreEqual(2.35, stop.Longitude, 0.0001);
        Assert.AreEqual(48.85, stop.Latitude, 0.0001);
        Assert.AreEqual("28800", stop.ArrivalTime);
        Assert.AreEqual(1, stop.SequenceOrder);
    }

    [TestMethod]
    public async Task FindById_PassesRouteIdFilter()
    {
        var useCase = new Mock<IGetAdjustedRoutesUseCase>();
        useCase.Setup(u => u.ExecuteAsync(It.Is<RouteFilterDto>(f => f.RouteId == 5)))
            .ReturnsAsync(SampleRoutes());
        var service = new TripService(useCase.Object);

        var response = await service.FindById(new RouteRequest { Id = 5 }, new FakeServerCallContext());

        Assert.AreEqual(1, response.Routes.Count);
        useCase.Verify(u => u.ExecuteAsync(It.Is<RouteFilterDto>(f => f.RouteId == 5)), Times.Once);
    }

    [TestMethod]
    public async Task FindAll_NullNames_AreReplacedByEmptyStrings()
    {
        var routes = new List<GetCompleteRouteDto>
        {
            new()
            {
                RouteId = 1,
                RouteShortName = null!,
                RouteLongName = null!,
                RouteTypeName = null!,
                Trips =
                [
                    new GetTripDetailsDto
                    {
                        TripId = 2,
                        Stops = [new GetStopDetailsDto { StopId = 3, StopName = null! }]
                    }
                ]
            }
        };
        var useCase = new Mock<IGetAdjustedRoutesUseCase>();
        useCase.Setup(u => u.ExecuteAsync(It.IsAny<RouteFilterDto>())).ReturnsAsync(routes);
        var service = new TripService(useCase.Object);

        var response = await service.FindAll(new Empty(), new FakeServerCallContext());

        var route = response.Routes[0];
        Assert.AreEqual("", route.RouteShortName);
        Assert.AreEqual("", route.RouteLongName);
        Assert.AreEqual("", route.RouteTypeName);
        Assert.AreEqual("", route.Trips[0].Stops[0].StopName);
    }

    [TestMethod]
    public async Task FindAll_EmptyRoutes_ReturnsEmptyResponse()
    {
        var useCase = new Mock<IGetAdjustedRoutesUseCase>();
        useCase.Setup(u => u.ExecuteAsync(It.IsAny<RouteFilterDto>()))
            .ReturnsAsync([]);
        var service = new TripService(useCase.Object);

        var response = await service.FindAll(new Empty(), new FakeServerCallContext());

        Assert.AreEqual(0, response.Routes.Count);
    }
}
