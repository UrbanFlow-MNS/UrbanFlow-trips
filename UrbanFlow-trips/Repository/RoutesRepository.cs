using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RoutesRepository(TripsDbContext dbContext)
{
    public async Task CreateRouteAsync(CreateRouteDTO routeDto)
    {
        Routes route = new Routes()
        {
            AgencyId = routeDto.AgencyId,
            RouteTypeId = routeDto.RouteTypeId,
            RouteShortName = routeDto.RouteShortName,
            RouteLongName = routeDto.RouteLongName
        };
        
        await dbContext.Routes.AddAsync(route);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Routes>> GetAllRoutesAsync()
    {
        return await dbContext.Routes.ToListAsync();
    }
}