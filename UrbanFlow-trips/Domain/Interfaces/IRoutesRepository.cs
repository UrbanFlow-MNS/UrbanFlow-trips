using UrbanFlow_trips.Application.DTO.CompleteRoute;
using UrbanFlow_trips.Application.DTO.Route;

namespace UrbanFlow_trips.Domain.Interfaces;

public interface IRoutesRepository
{
    Task CreateRouteAsync(CreateRouteDto routeDto);
    Task<List<GetCompleteRouteDto>> GetCompleteRouteByIdAsync(int id);
    Task<List<GetCompleteRouteDto>> GetRoutesFilter(RouteFilterDto filter);
    Task<List<GetRouteDto>> GetAllRoutesAsync();
    Task UpdateRouteAsync(int id, UpdateRouteDto routeDto);
    Task DeleteRouteAsync(int id);
    Task<List<GetCompleteRouteDto>> GetAllCompleteRoutesAsync();
    Task<bool> RouteExistsAsync(int id, CancellationToken cancellationToken = default);
}