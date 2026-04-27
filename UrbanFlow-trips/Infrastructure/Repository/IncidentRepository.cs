using AutoMapper;
using UrbanFlow_trips.Application.Records;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO.Incident;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Infrastructure.Repository;

public class IncidentRepository(TripsDbContext dbContext, IMapper mapper)
{
    public async Task CreateIncidentAsync(CreateIncidentDto incidentDto)
    {
        ArgumentNullException.ThrowIfNull(incidentDto);

        var incident = mapper.Map<Incident>(incidentDto);
        await dbContext.Incidents.AddAsync(incident);
        await dbContext.SaveChangesAsync();
    }
}