using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RouteTypeRepository(TripsDbContext dbContext)
{
    public async Task CreateRouteTypeAsync(CreateRouteTypeDTO routeTypeDto)
    {
        RouteType routeType = new RouteType()
        {
            RouteTypeName = routeTypeDto.RouteTypeName
        };
        
        await dbContext.RouteTypes.AddAsync(routeType);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<RouteType>> GetAllRouteTypesAsync()
    {
        return await dbContext.RouteTypes.ToListAsync();
    }
}