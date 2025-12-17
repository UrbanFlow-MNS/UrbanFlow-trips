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
    
    // Jointure de la mort mais je vois pas comment faire autrement
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
        var query = dbContext.Routes.AsQueryable();
        
        if (filter.AgencyId != null)
            query = query.Where(route => route.AgencyId == filter.AgencyId);
        
        if (filter.RouteTypeId != null)
            query = query.Where(route => route.RouteTypeId == filter.RouteTypeId);
        
        if (filter.RouteId != null)
            query = query.Where(route => route.RouteId == filter.RouteId);
        
        
        await query.ToListAsync();
        return mapper.Map<List<GetRouteDTO>>(query);
    }

    public async Task<List<GetRouteDTO>> GetAllRoutesAsync()
    {
        var routes =  await dbContext.Routes.ToListAsync();
        return mapper.Map<List<GetRouteDTO>>(routes);
    }
}