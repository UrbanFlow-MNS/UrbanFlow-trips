using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface IRouteTypeRepository
{
    Task CreateRouteTypeAsync(CreateRouteTypeDto routeTypeDto);
    Task<List<GetRouteTypeDto>> GetAllRouteTypesAsync();
}