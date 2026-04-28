using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Application.Records;
using UrbanFlow_trips.Domain.Entities;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.Infrastructure.Database;

namespace UrbanFlow_trips.Infrastructure.Repository;

public class IncidentRepository(TripsDbContext dbContext, IMapper mapper) : IIncidentRepository
{
    public async Task CreateIncidentAsync(CreateIncidentDto incidentDto)
    {
        ArgumentNullException.ThrowIfNull(incidentDto);

        var incident = mapper.Map<Incident>(incidentDto);
        await dbContext.Incidents.AddAsync(incident);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Incident?> DeleteIncidentAsync(int id)
    {
        var incident = await dbContext.Incidents.FindAsync(id);
        if (incident is null) return null;

        dbContext.Incidents.Remove(incident);
        await dbContext.SaveChangesAsync();
        return incident;
    }
    public async Task<int?> EstimateMinutesLateByRouteId(int routeId)
    {
        var incident = await dbContext.Incidents
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.RouteId == routeId);

        return incident?.EstimateDuration > 0 ? incident.EstimateDuration : null;
    }

    public bool IsRouteIdImpacted(int routeId)
    {
        return dbContext.Incidents.Any(route => route.RouteId == routeId);
    }
}