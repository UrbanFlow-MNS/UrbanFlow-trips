using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RoutesRepository(TripsDbContext dbContext, IMapper mapper) : IRoutesRepository
{
    public async Task CreateRouteAsync(CreateRouteDTO routeDto)
    {
        var route = mapper.Map<Routes>(routeDto);
        
        
        await dbContext.Routes.AddAsync(route);
        await dbContext.SaveChangesAsync();
    }

    private async Task<Routes?> GetRouteByIdAsync(int id)
    {
        return await dbContext.Routes.FindAsync(id);
    }
    
    public async Task<GetCompleteRouteDTO?> GetCompleteRouteAsync(int id)
    {
        return await dbContext.Routes
            .AsNoTracking()
            .Where(r => r.RouteId == id)
            .Select(r => new GetCompleteRouteDTO
            {
                RouteId = r.RouteId,
                RouteShortName = r.RouteShortName,
                RouteLongName = r.RouteLongName,
                RouteTypeName = r.RouteTypeId.ToString(),

                Trips = r.Trips
                    .Select(t => new GetTripDetailsDTO()
                    {
                        TripId = t.TripId,

                        Stops = t.StopTrips
                            .OrderBy(st => st.StopSequence)
                            .Select(st => new GetStopDetailsDTO()
                            {
                                StopId = st.StopId,
                                StopName = st.Stop.StopName,
                                Longitude = st.Stop.StopLong,
                                Latitude = st.Stop.StopLat,
                                ArrivalTime = st.ArrivalTime,
                                SequenceOrder = st.StopSequence
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
    

    public async Task<List<GetRouteDTO>> GetRoutesFilter(RouteFilterDTO filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var query = dbContext.Routes.AsQueryable();
        
        if (filter.AgencyId != null)
            query = query.Where(route => route.AgencyId == filter.AgencyId);
        
        if (filter.RouteTypeId != null)
            query = query.Where(route => route.RouteTypeId == filter.RouteTypeId);
        
        if (filter.RouteId != null)
            query = query.Where(route => route.RouteId == filter.RouteId);
        
        
        await query.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetRouteDTO>>(query);
    }

    public async Task UpdateRouteAsync(int id, UpdateRouteDTO routeDto)
    {
        var route = await GetRouteByIdAsync(id);
        
        if (route == null)
            throw new KeyNotFoundException($"Route with id {id} not found");
        
        mapper.Map(routeDto, route);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GetRouteDTO>> GetAllRoutesAsync()
    {
        var routes =  await dbContext.Routes.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetRouteDTO>>(routes);
    }
}