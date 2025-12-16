using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RoutesRepository(TripsDbContext dbContext, IMapper _mapper)
{
    public async Task CreateRouteAsync(CreateRouteDTO routeDto)
    {
        var route = _mapper.Map<Routes>(routeDto);
        
        
        await dbContext.Routes.AddAsync(route);
        await dbContext.SaveChangesAsync();
    }
    

    public async Task<List<GetRouteDTO>> GetRoutesFilter(RouteFilterDTO filter)
    {
        var query = dbContext.Routes.AsQueryable();
        
        if (filter.AgencyId != null)
            query = query.Where(route => route.AgencyId == filter.AgencyId);
        
        if (filter.RouteTypeId != null)
            query = query.Where(route => route.RouteTypeId == filter.RouteTypeId);
        
        if (filter.RouteId != null)
            query = query.Where(route => route.RouteId == filter.RouteId);
        
        
        await query.ToListAsync();
        return _mapper.Map<List<GetRouteDTO>>(query);
    }

    public async Task<List<GetRouteDTO>> GetAllRoutesAsync()
    {
        var routes =  await dbContext.Routes.ToListAsync();
        return _mapper.Map<List<GetRouteDTO>>(routes);
    }
}