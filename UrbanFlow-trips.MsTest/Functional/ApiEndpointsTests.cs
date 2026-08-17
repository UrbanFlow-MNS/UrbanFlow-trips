using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UrbanFlow_trips.Application.DTO;
using UrbanFlow_trips.Application.DTO.Calendar;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Application.DTO.Stop;

namespace UrbanFlow_trips.MsTest.Functional;

[TestClass]
public class ApiEndpointsTests
{
    private static CustomWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void ClassInit(TestContext _)
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task Root_ReturnsGrpcInformationMessage()
    {
        var response = await _client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        StringAssert.Contains(body, "gRPC");
    }

    [TestMethod]
    public async Task Metrics_ReturnsPrometheusText()
    {
        var response = await _client.GetAsync("/metrics");

        response.EnsureSuccessStatusCode();
        StringAssert.Contains(response.Content.Headers.ContentType!.ToString(), "text/plain");
    }

    [TestMethod]
    public async Task Calendar_CreateAndGetAll_Works()
    {
        var create = await _client.PostAsJsonAsync("/api/Calendar/create", new CreateCalendarDto
        {
            Monday = true,
            StartDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 12, 31)
        });
        Assert.AreEqual(HttpStatusCode.OK, create.StatusCode);

        var all = await _client.GetFromJsonAsync<List<GetCalendarDto>>("/api/Calendar/all");
        Assert.IsNotNull(all);
        Assert.IsTrue(all.Count >= 1);
    }

    [TestMethod]
    public async Task Routes_FullCrudFlow_Works()
    {
        // Create
        var create = await _client.PostAsJsonAsync("/api/Routes/create", new CreateRouteDto
        {
            AgencyId = 1,
            RouteShortName = "F1",
            RouteLongName = "Ligne fonctionnelle",
            RouteTypeId = 2
        });
        Assert.AreEqual(HttpStatusCode.OK, create.StatusCode);

        // GetAll
        var all = await _client.GetFromJsonAsync<List<GetRouteDto>>("/api/Routes/all");
        Assert.IsNotNull(all);
        var routeId = all.First(r => r.RouteShortName == "F1").RouteId;

        // GetDetails
        var details = await _client.GetAsync($"/api/Routes/getDetails/{routeId}");
        Assert.AreEqual(HttpStatusCode.OK, details.StatusCode);

        // GetAllCompleteRoutes
        var complete = await _client.GetAsync("/api/Routes/getAllCompleteRoutes");
        Assert.AreEqual(HttpStatusCode.OK, complete.StatusCode);

        // Filtre via le use case (incident inexistant => pas d'ajustement)
        var filtered = await _client.GetAsync($"/api/Routes?RouteId={routeId}&AgencyId=1&RouteTypeId=2");
        Assert.AreEqual(HttpStatusCode.OK, filtered.StatusCode);
        var filteredBody = await filtered.Content.ReadAsStringAsync();
        StringAssert.Contains(filteredBody, "F1");

        // Update
        var update = await _client.PutAsJsonAsync($"/api/Routes/update/{routeId}", new UpdateRouteDto
        {
            RouteShortName = "F2",
            RouteLongName = "Ligne renommée",
            RouteTypeId = 3
        });
        Assert.AreEqual(HttpStatusCode.OK, update.StatusCode);

        // Delete
        var delete = await _client.DeleteAsync($"/api/Routes/delete/{routeId}");
        Assert.AreEqual(HttpStatusCode.OK, delete.StatusCode);
    }

    [TestMethod]
    public async Task Routes_Create_InvalidDto_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/Routes/create", new
        {
            AgencyId = 1,
            RouteShortName = "",
            RouteLongName = "",
            RouteTypeId = 0
        });

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Stops_FullCrudFlow_Works()
    {
        var create = await _client.PostAsJsonAsync("/api/Stops/create", new CreateStopDto
        {
            StopName = "Arrêt fonctionnel",
            StopLat = 48.85m,
            StopLong = 2.35m,
            AgencyId = 1
        });
        Assert.AreEqual(HttpStatusCode.OK, create.StatusCode);

        var all = await _client.GetFromJsonAsync<List<GetStopDto>>("/api/Stops/all");
        Assert.IsNotNull(all);
        var stopId = all.First(s => s.StopName == "Arrêt fonctionnel").StopId;

        var update = await _client.PutAsJsonAsync($"/api/Stops/update/{stopId}", new UpdateStopDto
        {
            StopName = "Arrêt renommé",
            StopLat = 45m,
            StopLong = 4m
        });
        Assert.AreEqual(HttpStatusCode.OK, update.StatusCode);

        var delete = await _client.DeleteAsync($"/api/Stops/delete/{stopId}");
        Assert.AreEqual(HttpStatusCode.OK, delete.StatusCode);
    }

    [TestMethod]
    public async Task Stops_Create_InvalidLatitude_Returns400()
    {
        var response = await _client.PostAsJsonAsync("/api/Stops/create", new
        {
            StopName = "Invalide",
            StopLat = 120m,
            StopLong = 2m,
            AgencyId = 1
        });

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Trip_FullFlow_CreateUpdateDelete_Works()
    {
        // Pré-requis : calendrier, route, arrêts
        (await _client.PostAsJsonAsync("/api/Calendar/create", new CreateCalendarDto
        {
            Monday = true,
            StartDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 12, 31)
        })).EnsureSuccessStatusCode();

        (await _client.PostAsJsonAsync("/api/Routes/create", new CreateRouteDto
        {
            AgencyId = 1,
            RouteShortName = "T",
            RouteLongName = "Ligne trajets",
            RouteTypeId = 1
        })).EnsureSuccessStatusCode();

        (await _client.PostAsJsonAsync("/api/Stops/create", new CreateStopDto
        {
            StopName = "Départ",
            StopLat = 1m,
            StopLong = 1m,
            AgencyId = 1
        })).EnsureSuccessStatusCode();

        var calendars = await _client.GetFromJsonAsync<List<GetCalendarDto>>("/api/Calendar/all");
        var routes = await _client.GetFromJsonAsync<List<GetRouteDto>>("/api/Routes/all");
        var stops = await _client.GetFromJsonAsync<List<GetStopDto>>("/api/Stops/all");
        var serviceId = calendars!.Last().ServiceId;
        var routeId = routes!.First(r => r.RouteShortName == "T").RouteId;
        var stopId = stops!.First(s => s.StopName == "Départ").StopId;

        // Create trip
        var createTrip = await _client.PostAsJsonAsync("/api/Trip/create", new
        {
            RouteId = routeId,
            ServiceId = serviceId,
            TripHeadsign = "Fonctionnel",
            CreateStopTrips = new[]
            {
                new { StopId = stopId, ArrivalTime = "08:00:00", DepartureTime = "07:55:00" }
            }
        });
        Assert.AreEqual(HttpStatusCode.OK, createTrip.StatusCode);

        // Retrouve le tripId via les routes complètes
        var detailsJson = await _client.GetStringAsync($"/api/Routes/getDetails/{routeId}");
        using var doc = JsonDocument.Parse(detailsJson);
        var tripId = doc.RootElement[0].GetProperty("trips")[0].GetProperty("tripId").GetInt32();

        // Update horaires
        var updateHourly = await _client.PutAsJsonAsync($"/api/Trip/updateHourly/{stopId}/{tripId}", new
        {
            ArrivalTime = "09:00:00",
            DepartureTime = "08:50:00"
        });
        Assert.AreEqual(HttpStatusCode.OK, updateHourly.StatusCode);

        // Update service
        var updateService = await _client.PutAsJsonAsync($"/api/Trip/updateService/{tripId}", new
        {
            ServiceId = serviceId
        });
        Assert.AreEqual(HttpStatusCode.OK, updateService.StatusCode);

        // Delete stop trip puis trip
        var deleteStopTrip = await _client.DeleteAsync($"/api/Trip/delete/{stopId}/{tripId}");
        Assert.AreEqual(HttpStatusCode.OK, deleteStopTrip.StatusCode);

        var deleteTrip = await _client.DeleteAsync($"/api/Trip/delete/{tripId}");
        Assert.AreEqual(HttpStatusCode.OK, deleteTrip.StatusCode);
    }

    [TestMethod]
    public async Task Logs_Test_PublishesWithoutError()
    {
        var response = await _client.PostAsJsonAsync("/Logs/test", new LogMessageDto
        {
            MicroserviceName = "trips",
            CodeOfEvent = 200,
            Event = "functional-test"
        });

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
