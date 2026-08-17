using System.Text;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UrbanFlow_trips.API.Consumers;
using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.Consumers;

[TestClass]
public class IncidentConsumerTests
{
    private static ConsumeContext<Application.Records.CreateIncidentRecord> CreateContext(string rawJson)
    {
        var bytes = Encoding.UTF8.GetBytes(rawJson);
        var body = new Mock<MessageBody>();
        body.Setup(b => b.GetBytes()).Returns(bytes);
        body.Setup(b => b.GetStream()).Returns(() => new MemoryStream(bytes));
        body.Setup(b => b.GetString()).Returns(rawJson);

        var receiveContext = new Mock<ReceiveContext>();
        receiveContext.SetupGet(r => r.Body).Returns(body.Object);

        var context = new Mock<ConsumeContext<Application.Records.CreateIncidentRecord>>();
        context.SetupGet(c => c.ReceiveContext).Returns(receiveContext.Object);
        return context.Object;
    }

    private static IncidentConsumer CreateConsumer(Mock<IIncidentRepository> repo)
        => new(NullLogger<IncidentConsumer>.Instance, repo.Object);

    [TestMethod]
    public async Task Consume_IncidentCreated_CreatesOneIncidentPerAffectedRoute()
    {
        var repo = new Mock<IIncidentRepository>();
        const string json = """
        {
            "pattern": "incident.created",
            "data": {
                "incidentId": 12,
                "siteId": 3,
                "estimateDuration": 20,
                "priority": "high",
                "status": "open",
                "affectedRouteIds": [1, 2, 3]
            }
        }
        """;

        await CreateConsumer(repo).Consume(CreateContext(json));

        repo.Verify(r => r.CreateIncidentAsync(It.Is<CreateIncidentDto>(
            d => d.IncidentId == 12 && d.EstimateDuration == 20)), Times.Exactly(3));
        repo.Verify(r => r.CreateIncidentAsync(It.Is<CreateIncidentDto>(d => d.RouteId == 1)), Times.Once);
        repo.Verify(r => r.CreateIncidentAsync(It.Is<CreateIncidentDto>(d => d.RouteId == 2)), Times.Once);
        repo.Verify(r => r.CreateIncidentAsync(It.Is<CreateIncidentDto>(d => d.RouteId == 3)), Times.Once);
    }

    [TestMethod]
    public async Task Consume_IncidentClosed_DeletesIncident()
    {
        var repo = new Mock<IIncidentRepository>();
        repo.Setup(r => r.DeleteIncidentAsync(12)).ReturnsAsync((Incident?)null);
        const string json = """
        {
            "pattern": "incident.closed",
            "data": { "incidentId": 12, "affectedRouteIds": [] }
        }
        """;

        await CreateConsumer(repo).Consume(CreateContext(json));

        repo.Verify(r => r.DeleteIncidentAsync(12), Times.Once);
    }

    [TestMethod]
    public async Task Consume_UnknownPattern_DoesNothing()
    {
        var repo = new Mock<IIncidentRepository>(MockBehavior.Strict);
        const string json = """
        {
            "pattern": "incident.unknown",
            "data": { "incidentId": 1, "affectedRouteIds": [] }
        }
        """;

        await CreateConsumer(repo).Consume(CreateContext(json));

        repo.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task Consume_NullPattern_IsIgnored()
    {
        var repo = new Mock<IIncidentRepository>(MockBehavior.Strict);
        const string json = """
        {
            "pattern": null,
            "data": { "incidentId": 1, "affectedRouteIds": [] }
        }
        """;

        await CreateConsumer(repo).Consume(CreateContext(json));

        repo.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task Consume_NullData_IsIgnored()
    {
        var repo = new Mock<IIncidentRepository>(MockBehavior.Strict);
        const string json = """
        {
            "pattern": "incident.created",
            "data": null
        }
        """;

        await CreateConsumer(repo).Consume(CreateContext(json));

        repo.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task Consume_NullWrapper_IsIgnored()
    {
        var repo = new Mock<IIncidentRepository>(MockBehavior.Strict);

        await CreateConsumer(repo).Consume(CreateContext("null"));

        repo.VerifyNoOtherCalls();
    }
}
