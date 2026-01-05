using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface IAgencyRepository
{
    Task CreateAgencyAsync(CreateAgencyDTO agencyDto);
    Task<List<GetAgencyDTO>> GetAllAgenciesAsync();
    Task UpdateAgencyAsync(int id, UpdateAgencyDTO agency);
}