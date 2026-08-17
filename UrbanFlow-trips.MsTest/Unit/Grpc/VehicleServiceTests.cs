using Grpc.Core;
using Microsoft.Extensions.Logging.Abstractions;
using UrbanFlow_trips.API.GrpcServices;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Unit.Grpc;

[TestClass]
public class VehicleServiceTests
{
    private static VehicleService CreateService(Moq.Mock<Vehicler.VehiclerClient> clientMock)
        => new(clientMock.Object, NullLogger<VehicleService>.Instance);

    [TestMethod]
    public async Task GetVehicleName_Success_ReturnsName()
    {
        var service = CreateService(GrpcHelpers.CreateVehiclerClientMock("Bus"));

        var name = await service.GetVehicleNameByRouteTypeIdAsync(1);

        Assert.AreEqual("Bus", name);
    }

    [TestMethod]
    public async Task GetVehicleName_Unavailable_ReturnsNull()
    {
        var service = CreateService(GrpcHelpers.CreateThrowingVehiclerClientMock(
            new RpcException(new Status(StatusCode.Unavailable, "down"))));

        Assert.IsNull(await service.GetVehicleNameByRouteTypeIdAsync(1));
    }

    [TestMethod]
    public async Task GetVehicleName_DeadlineExceeded_ReturnsNull()
    {
        var service = CreateService(GrpcHelpers.CreateThrowingVehiclerClientMock(
            new RpcException(new Status(StatusCode.DeadlineExceeded, "timeout"))));

        Assert.IsNull(await service.GetVehicleNameByRouteTypeIdAsync(2));
    }

    [TestMethod]
    public async Task GetVehicleName_OtherRpcException_ReturnsNull()
    {
        var service = CreateService(GrpcHelpers.CreateThrowingVehiclerClientMock(
            new RpcException(new Status(StatusCode.NotFound, "not found"))));

        Assert.IsNull(await service.GetVehicleNameByRouteTypeIdAsync(3));
    }

    [TestMethod]
    public async Task GetVehicleName_UnexpectedException_ReturnsNull()
    {
        var service = CreateService(GrpcHelpers.CreateThrowingVehiclerClientMock(
            new InvalidOperationException("boom")));

        Assert.IsNull(await service.GetVehicleNameByRouteTypeIdAsync(4));
    }
}
