using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface IRoutesRepository
{
    Task CreateRouteAsync(CreateRouteDTO routeDto);
    Task<GetCompleteRouteDTO?> GetCompleteRouteAsync(int id);
    Task<List<GetRouteDTO>> GetRoutesFilter(RouteFilterDTO filter);
    Task<List<GetRouteDTO>> GetAllRoutesAsync();
}