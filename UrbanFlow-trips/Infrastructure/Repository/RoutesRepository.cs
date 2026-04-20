using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;
using UrbanFlow_trips.Services;

namespace UrbanFlow_trips.Repository;

public class RoutesRepository(TripsDbContext dbContext, IMapper mapper, VehicleService vService) : IRoutesRepository
{
    private async Task<Routes?> GetRouteByIdAsync(int id) =>
        await dbContext.Routes.FindAsync(id);

    private IQueryable<dynamic> BuildCompleteRouteQuery() =>
        dbContext.Routes
            .AsNoTracking()
            .Select(r => new
            {
                r.RouteId,
                r.RouteShortName,
                r.RouteLongName,
                r.RouteTypeId,
                Trips = r.Trips.Select(t => new GetTripDetailsDto
                {
                    TripId = t.TripId,
                    Stops = t.StopTrips
                        .OrderBy(st => st.StopSequence)
                        .Select(st => new GetStopDetailsDto
                        {
                            StopId = st.StopId,
                            StopName = st.Stop.StopName,
                            Longitude = st.Stop.StopLong,
                            Latitude = st.Stop.StopLat,
                            ArrivalTime = st.ArrivalTime.ToTimeSpan().TotalSeconds,
                            SequenceOrder = st.StopSequence
                        })
                        .ToList()
                }).ToList()
            });

    private async Task<List<GetCompleteRouteDto>> EnrichWithVehicleNamesAsync(IEnumerable<dynamic> routes)
    {
        var tasks = routes.Select(async r => new GetCompleteRouteDto
        {
            RouteId = r.RouteId,
            RouteShortName = r.RouteShortName,
            RouteLongName = r.RouteLongName,
            RouteTypeName = await vService.GetVehicleNameByRouteTypeIdAsync(r.RouteTypeId) ?? "Inconnu",
            Trips = r.Trips
        });

        return [.. await Task.WhenAll(tasks)];
    }


    public async Task<List<GetCompleteRouteDto>> GetAllCompleteRoutesAsync()
    {
        var routes = await BuildCompleteRouteQuery().ToListAsync();
        return await EnrichWithVehicleNamesAsync(routes);
    }

    public async Task<List<GetCompleteRouteDto>> GetCompleteRouteByIdAsync(int id)
    {
        var routes = await BuildCompleteRouteQuery()
            .Where(r => r.AgencyId == id)
            .ToListAsync();

        return await EnrichWithVehicleNamesAsync(routes);
    }

    public async Task<List<GetRouteDto>> GetAllRoutesAsync()
    {
        var routes = await dbContext.Routes.AsNoTracking().ToListAsync();
        var dtos = mapper.Map<List<GetRouteDto>>(routes);
        return await EnrichRouteDtosAsync(dtos, routes.Select(r => r.RouteTypeId).ToList());
    }

    public async Task<List<GetRouteDto>> GetRoutesFilter(RouteFilterDto filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var query = dbContext.Routes.AsNoTracking().AsQueryable();

        if (filter.AgencyId != null)
            query = query.Where(r => r.AgencyId == filter.AgencyId);

        if (filter.RouteTypeId != null)
            query = query.Where(r => r.RouteTypeId == filter.RouteTypeId);

        if (filter.RouteId != null)
            query = query.Where(r => r.RouteId == filter.RouteId);

        var routes = await query.ToListAsync();
        var dtos = mapper.Map<List<GetRouteDto>>(routes);
        return await EnrichRouteDtosAsync(dtos, routes.Select(r => r.RouteTypeId).ToList());
    }

    private async Task<List<GetRouteDto>> EnrichRouteDtosAsync(List<GetRouteDto> dtos, List<int> routeTypeIds)
    {
        var tasks = dtos.Zip(routeTypeIds, async (dto, typeId) =>
        {
            dto.RouteTypeName = await vService.GetVehicleNameByRouteTypeIdAsync(typeId) ?? "Inconnu";
            return dto;
        });

        return [.. await Task.WhenAll(tasks)];
    }

    public async Task CreateRouteAsync(CreateRouteDto routeDto)
    {
        var route = mapper.Map<Routes>(routeDto);
        await dbContext.Routes.AddAsync(route);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRouteAsync(int id, UpdateRouteDto routeDto)
    {
        var route = await GetRouteByIdAsync(id)
            ?? throw new KeyNotFoundException($"Route with id {id} not found");

        mapper.Map(routeDto, route);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRouteAsync(int id)
    {
        var route = await GetRouteByIdAsync(id)
            ?? throw new KeyNotFoundException($"Route with id {id} not found");

        dbContext.Routes.Remove(route);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> RouteExistsAsync(int id, CancellationToken cancellationToken = default) =>
        await dbContext.Routes.AnyAsync(x => x.RouteId == id, cancellationToken);
}