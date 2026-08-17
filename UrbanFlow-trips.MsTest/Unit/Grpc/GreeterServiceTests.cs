using Microsoft.Extensions.Logging.Abstractions;
using UrbanFlow_trips.API.GrpcServices;
using UrbanFlow_trips.GrpcService;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Unit.Grpc;

[TestClass]
public class GreeterServiceTests
{
    [TestMethod]
    public async Task SayHello_ReturnsGreetingWithName()
    {
        var service = new GreeterService(NullLogger<GreeterService>.Instance);

        var reply = await service.SayHello(new HelloRequest { Name = "UrbanFlow" }, new FakeServerCallContext());

        Assert.AreEqual("Hello UrbanFlow", reply.Message);
    }
}
