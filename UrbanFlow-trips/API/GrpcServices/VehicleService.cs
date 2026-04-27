using Grpc.Core;

namespace UrbanFlow_trips.Services;

public class VehicleService
{
    private readonly Vehicler.VehiclerClient _client;
    private readonly ILogger<VehicleService> _logger;



    public VehicleService(Vehicler.VehiclerClient client, ILogger<VehicleService> logger)
    {
        _client = client;
        _logger = logger;
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
        catch (RpcException ex) when (ex.StatusCode is StatusCode.Unavailable or StatusCode.DeadlineExceeded)
        {
            _logger.LogWarning("Vehicle service indisponible pour routeTypeId {Id}", routeTypeId);
            return null;
        }
        catch (RpcException ex)
        {
            _logger.LogError("RpcException inattendue: {StatusCode} - {Message}", ex.StatusCode, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception inattendue: {Type} - {Message}", ex.GetType().Name, ex.Message);
            return null;
        }
    }
}