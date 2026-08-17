using MassTransit;
using Moq;
using UrbanFlow_trips.API.Controllers;
using UrbanFlow_trips.Application.DTO;

namespace UrbanFlow_trips.MsTest.Unit.Controllers;

[TestClass]
public class LogsControllerTests
{
    [TestMethod]
    public async Task TestLogs_PublishesMessage()
    {
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var controller = new LogsController(publishEndpoint.Object);
        var dto = new LogMessageDto { MicroserviceName = "trips", CodeOfEvent = 200, Event = "test" };

        await controller.TestLogs(dto);

        publishEndpoint.Verify(p => p.Publish(dto, It.IsAny<CancellationToken>()), Times.Once);
    }
}
