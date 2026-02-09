using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface IRoutesRepository
{
    Task CreateRouteAsync(CreateRouteDto routeDto);
    Task<GetCompleteRouteDto?> GetCompleteRouteByIdAsync(int id);
    Task<List<GetRouteDto>> GetRoutesFilter(RouteFilterDto filter);
    Task<List<GetRouteDto>> GetAllRoutesAsync();
    Task UpdateRouteAsync(int id, UpdateRouteDto routeDto);
    Task DeleteRouteAsync(int id);
    Task<List<GetCompleteRouteDto>> GetAllCompleteRoutesAsync();
    Task<bool> RouteExistsAsync(int id, CancellationToken cancellationToken = default);
}