using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Exceptions;
using UrbanFlow_trips.Models;
using UrbanFlow_trips.Services;

namespace UrbanFlow_trips.Repository;

public class RoutesRepository(TripsDbContext dbContext, IMapper mapper, VehicleService vService) : IRoutesRepository
{
    public async Task CreateRouteAsync(CreateRouteDto routeDto)
    {
        var route = mapper.Map<Routes>(routeDto);
        await dbContext.Routes.AddAsync(route);
        await dbContext.SaveChangesAsync();
    }

    private async Task<Routes?> GetRouteByIdAsync(int id)
    {
        return await dbContext.Routes.FindAsync(id);
    }

    public Task<List<GetCompleteRouteDto>> GetAllCompleteRoutesAsync()
        => GetRoutesFilter(new RouteFilterDto());

    
    public Task<List<GetCompleteRouteDto>> GetCompleteRouteByIdAsync(int routeId)
        => GetRoutesFilter(new RouteFilterDto { RouteId = routeId });

    private async Task<List<GetCompleteRouteDto>> MappingRoutes(IQueryable<Routes> query)
    {
        var routes = await query
            .Select(r => new
            {
                r.RouteId,
                r.RouteTypeId,
                r.RouteShortName,
                r.RouteLongName,
                Trips = r.Trips.Select(t => new
                {
                    t.TripId,
                    Stops = t.StopTrips
                        .OrderBy(st => st.StopSequence)
                        .Select(st => new
                        {
                            st.StopId,
                            st.Stop.StopName,
                            st.Stop.StopLong,
                            st.Stop.StopLat,
                            st.ArrivalTime,
                            st.StopSequence
                        })
                })
            })
            .ToListAsync();

        var tasks = routes.Select(async r => new GetCompleteRouteDto
        {
            RouteId = r.RouteId,
            RouteShortName = r.RouteShortName,
            RouteLongName = r.RouteLongName,
            RouteTypeName = await vService.GetVehicleNameByRouteTypeIdAsync(r.RouteTypeId) ?? "null",
            Trips = r.Trips.Select(t => new GetTripDetailsDto
            {
                TripId = t.TripId,
                Stops = t.Stops.Select(st => new GetStopDetailsDto
                {
                    StopId = st.StopId,
                    StopName = st.StopName,
                    Longitude = st.StopLong,
                    Latitude = st.StopLat,
                    ArrivalTime = st.ArrivalTime.ToTimeSpan().TotalSeconds,
                    SequenceOrder = st.StopSequence
                }).ToList()
            }).ToList()
        });
    return (await Task.WhenAll(tasks)).ToList();
    }

    public async Task<List<GetCompleteRouteDto>> GetRoutesFilter(RouteFilterDto filter)
    {
    ArgumentNullException.ThrowIfNull(filter);

    var query = dbContext.Routes.AsNoTracking().AsQueryable();

    if (filter.AgencyId != null)
        query = query.Where(route => route.AgencyId == filter.AgencyId);

    if (filter.RouteTypeId != null)
        query = query.Where(route => route.RouteTypeId == filter.RouteTypeId);

    if (filter.RouteId != null)
        query = query.Where(route => route.RouteId == filter.RouteId);

    return await MappingRoutes(query);
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
        var routes = await dbContext.Routes
            .AsNoTracking()
            .Select(r => new { r.RouteId, r.AgencyId, r.RouteTypeId, r.RouteShortName, r.RouteLongName })
            .ToListAsync();

        var tasks = routes.Select(async r => new GetRouteDto
        {
            RouteId = r.RouteId,
            AgencyId = r.AgencyId,
            RouteShortName = r.RouteShortName,
            RouteLongName = r.RouteLongName,
            RouteTypeName = await vService.GetVehicleNameByRouteTypeIdAsync(r.RouteTypeId) ?? "null"
        });

        return (await Task.WhenAll(tasks)).ToList();
    }
    
    public async Task<bool> RouteExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Routes.AnyAsync(x => x.RouteId == id, cancellationToken);
    }
}