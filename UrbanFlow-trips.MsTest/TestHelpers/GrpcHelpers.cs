using Grpc.Core;
using Moq;
using UrbanFlow_trips;

namespace UrbanFlow_trips.MsTest.TestHelpers;

public static class GrpcHelpers
{
    public static AsyncUnaryCall<TResponse> CreateAsyncUnaryCall<TResponse>(TResponse response)
    {
        return new AsyncUnaryCall<TResponse>(
            Task.FromResult(response),
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => new Metadata(),
            () => { });
    }

    public static Mock<Vehicler.VehiclerClient> CreateVehiclerClientMock(string vehicleName)
    {
        var mock = new Mock<Vehicler.VehiclerClient>();
        mock.Setup(c => c.FindByIdAsync(
                It.IsAny<VehicleRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .Returns(CreateAsyncUnaryCall(new VehicleResponse { VehicleName = vehicleName }));
        return mock;
    }

    public static Mock<Vehicler.VehiclerClient> CreateThrowingVehiclerClientMock(Exception exception)
    {
        var mock = new Mock<Vehicler.VehiclerClient>();
        mock.Setup(c => c.FindByIdAsync(
                It.IsAny<VehicleRequest>(),
                It.IsAny<Metadata>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .Throws(exception);
        return mock;
    }
}

public sealed class FakeServerCallContext : ServerCallContext
{
    protected override string MethodCore => "fake-method";
    protected override string HostCore => "localhost";
    protected override string PeerCore => "ipv4:127.0.0.1";
    protected override DateTime DeadlineCore => DateTime.UtcNow.AddMinutes(1);
    protected override Metadata RequestHeadersCore { get; } = new();
    protected override CancellationToken CancellationTokenCore => CancellationToken.None;
    protected override Metadata ResponseTrailersCore { get; } = new();
    protected override Status StatusCore { get; set; }
    protected override WriteOptions? WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore { get; } =
        new(null, new Dictionary<string, List<AuthProperty>>());

    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
        => throw new NotSupportedException();

    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        => Task.CompletedTask;
}
