using AutoMapper;
using UrbanFlow_trips.Application.Records;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO.Incident;
using UrbanFlow_trips.Models;

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
        return await dbContext.Incidents.FindAsync(id);
    }

    public int? EstimateMinutesLateByRouteId(int routeId)
    {
        int minutes = dbContext.Incidents.Find(routeId).EstimateDuration;
        
        return minutes;
    }
}