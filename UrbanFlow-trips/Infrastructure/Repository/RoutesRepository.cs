using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Exceptions;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class RoutesRepository(TripsDbContext dbContext, IMapper mapper, IAgencyRepository agencyRepository, IRouteTypeRepository routeTypeRepository) : IRoutesRepository
{
    public async Task CreateRouteAsync(CreateRouteDto routeDto)
    {
        var route = mapper.Map<Routes>(routeDto);
        
        if (!await agencyRepository.AgencyExistsAsync(routeDto.AgencyId))
            throw new NotFoundException($"Agency with id {routeDto.AgencyId} not found", 404);
        
        if (!await routeTypeRepository.RouteTypeExistsAsync(routeDto.RouteTypeId))
            throw new NotFoundException($"Agency with id {routeDto.RouteTypeId} not found", 404);
        
        await dbContext.Routes.AddAsync(route);
        await dbContext.SaveChangesAsync();
    }

    private async Task<Routes?> GetRouteByIdAsync(int id)
    {
        return await dbContext.Routes.FindAsync(id);
    }

    public async Task<List<GetCompleteRouteDto>> GetAllCompleteRoutesAsync()
    {
        return await dbContext.Routes
            .AsNoTracking()
            .Select(r => new GetCompleteRouteDto
            {
                RouteId = r.RouteId,
                RouteShortName = r.RouteShortName,
                RouteLongName = r.RouteLongName,
                RouteTypeName = r.RouteTypeId.ToString(),

                Trips = r.Trips
                    .Select(t => new GetTripDetailsDto()
                    {
                        TripId = t.TripId,

                        Stops = t.StopTrips
                            .OrderBy(st => st.StopSequence)
                            .Select(st => new GetStopDetailsDto()
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
            }).ToListAsync();
    }
    
    public async Task<GetCompleteRouteDto?> GetCompleteRouteByIdAsync(int id)
    {
        return await dbContext.Routes
            .AsNoTracking()
            .Where(r => r.RouteId == id)
            .Select(r => new GetCompleteRouteDto
            {
                RouteId = r.RouteId,
                RouteShortName = r.RouteShortName,
                RouteLongName = r.RouteLongName,
                RouteTypeName = r.RouteTypeId.ToString(),

                Trips = r.Trips
                    .Select(t => new GetTripDetailsDto()
                    {
                        TripId = t.TripId,

                        Stops = t.StopTrips
                            .OrderBy(st => st.StopSequence)
                            .Select(st => new GetStopDetailsDto()
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
    

    public async Task<List<GetRouteDto>> GetRoutesFilter(RouteFilterDto filter)
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
        return mapper.Map<List<GetRouteDto>>(query);
    }

    public async Task UpdateRouteAsync(int id, UpdateRouteDto routeDto)
    {
        var route = await GetRouteByIdAsync(id);
        
        if (route == null)
            throw new KeyNotFoundException($"Route with id {id} not found");
        
        mapper.Map(routeDto, route);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRouteAsync(int id)
    {
        var route = await GetRouteByIdAsync(id);
        
        if (route == null)
            throw new KeyNotFoundException($"Route with id {id} not found");
        
        dbContext.Routes.Remove(route);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<GetRouteDto>> GetAllRoutesAsync()
    {
        var routes =  await dbContext.Routes.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetRouteDto>>(routes);
    }
    
    public async Task<bool> RouteExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Routes.AnyAsync(x => x.RouteId == id, cancellationToken);
    }
}