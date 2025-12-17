using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RouteTypeRepository(TripsDbContext dbContext, IMapper _mapper)
{
    public async Task CreateRouteTypeAsync(CreateRouteTypeDTO routeTypeDto)
    {
        var routeType = _mapper.Map<RouteType>(routeTypeDto);
        await dbContext.RouteTypes.AddAsync(routeType);
        await dbContext.SaveChangesAsync();
    }
    

    public async Task<List<GetRouteTypeDTO>> GetAllRouteTypesAsync()
    {
        var routeType =  await dbContext.RouteTypes.ToListAsync();
        return _mapper.Map<List<GetRouteTypeDTO>>(routeType);
    }
}