using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Domain.Interfaces;

public interface IIncidentRepository
{
    Task CreateIncidentAsync(CreateIncidentDto incidentDto);
    Task<Incident?> DeleteIncidentAsync(int id);
    Task<int?> EstimateMinutesLateByRouteId(int routeId);
}