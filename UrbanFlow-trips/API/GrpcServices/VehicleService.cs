using Grpc.Core;

namespace UrbanFlow_trips.Services;

public class VehicleService
{
    private readonly Vehicler.VehiclerClient _client;

    public VehicleService(Vehicler.VehiclerClient client)
    {
        _client = client;
    }

    public async Task<string?> GetVehicleNameByRouteTypeIdAsync(int routeTypeId)
    {
        try
        {
            var response = await _client.FindByIdAsync(
                new VehicleRequest { Id = routeTypeId }
            );
            return response.VehicleName;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }
}