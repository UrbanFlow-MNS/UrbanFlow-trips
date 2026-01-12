using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface IAgencyRepository
{
    Task CreateAgencyAsync(CreateAgencyDto agencyDto);
    Task<List<GetAgencyDto>> GetAllAgenciesAsync();
    Task UpdateAgencyAsync(int id, UpdateAgencyDto agency);
    Task DeleteAgencyAsync(int id);
}