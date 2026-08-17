using MassTransit;
using Moq;
using UrbanFlow_trips.API.Consumers;
using UrbanFlow_trips.Application.DTO;

namespace UrbanFlow_trips.MsTest.Unit.Consumers;

[TestClass]
public class PostLogsConsumerTests
{
    [TestMethod]
    public async Task Consume_LogsMessage_CompletesWithoutError()
    {
        var context = new Mock<ConsumeContext<LogMessageDto>>();
        context.SetupGet(c => c.Message)
            .Returns(new LogMessageDto { MicroserviceName = "trips", CodeOfEvent = 1, Event = "ok" });

        await new PostLogsConsumer().Consume(context.Object);

        context.VerifyGet(c => c.Message, Times.Once);
    }
}
