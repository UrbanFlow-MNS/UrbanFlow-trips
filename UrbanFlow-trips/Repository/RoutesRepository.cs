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
    
    public async Task<GetCompleteRouteDTO?> GetCompleteRouteAsync(int id)
    {
        return await dbContext.Routes
            .Where(r => r.RouteId == id)
            .Select(r => new GetCompleteRouteDTO
            {
                RouteId = r.RouteId,
                RouteShortName = r.RouteShortName,
                RouteLongName = r.RouteLongName,
                RouteTypeName = r.RouteTypeId.ToString(),
                StopDetails = r.Trips
                    .SelectMany(t => t.StopTrips)
                    .OrderBy(st => st.StopSequence)
                    .Select(st => new GetStopDetailsDto
                    {
                        StopId = st.StopId,
                        StopName = st.Stop.StopName,
                        Latitude = st.Stop.StopLat,
                        ArrivalTime = st.ArrivalTime,
                        SequenceOrder = st.StopSequence
                    }).ToList()

            }).FirstOrDefaultAsync();
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