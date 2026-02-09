using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RouteTypeRepository(TripsDbContext dbContext, IMapper mapper) : IRouteTypeRepository
{
    public async Task CreateRouteTypeAsync(CreateRouteTypeDto routeTypeDto)
    {
        ArgumentNullException.ThrowIfNull(routeTypeDto);

        var routeType = mapper.Map<RouteType>(routeTypeDto);
        await dbContext.RouteTypes.AddAsync(routeType);
        await dbContext.SaveChangesAsync();
    }
    

    public async Task<List<GetRouteTypeDto>> GetAllRouteTypesAsync()
    {
        var routeType =  await dbContext.RouteTypes.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetRouteTypeDto>>(routeType);
    }
    
    public async Task<bool> RouteTypeExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.RouteTypes.AnyAsync(x => x.RouteTypeId == id, cancellationToken);
    }
}