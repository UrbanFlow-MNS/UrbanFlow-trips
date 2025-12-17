using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface IRouteTypeRepository
{
    Task CreateRouteTypeAsync(CreateRouteTypeDTO routeTypeDto);
    Task<List<GetRouteTypeDTO>> GetAllRouteTypesAsync();
}