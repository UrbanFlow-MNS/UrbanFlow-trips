using UrbanFlow_trips.DTO.Incident;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Infrastructure.Repository;

public interface IIncidentRepository
{
    Task CreateIncidentAsync(CreateIncidentDto incidentDto);
    Task<Incident?> DeleteIncidentAsync(int id);
}