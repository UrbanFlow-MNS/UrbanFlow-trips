using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class AgencyRepository(TripsDbContext dbcontext, IMapper mapper) : IAgencyRepository
{
    public async Task CreateAgencyAsync(CreateAgencyDTO agencyDto)
    {
        ArgumentNullException.ThrowIfNull(agencyDto);

        var agency = mapper.Map<Agency>(agencyDto);
        await dbcontext.Agencies.AddAsync(agency);
        await dbcontext.SaveChangesAsync();
    }

    private async Task<Agency?> GetAgencyByIdAsync(int id)
    {
        return await dbcontext.Agencies.FindAsync(id);
    }

    public async Task<List<GetAgencyDTO>> GetAllAgenciesAsync()
    {
        var agency = await dbcontext.Agencies.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetAgencyDTO>>(agency);
    }
    
    public async Task UpdateAgencyAsync(int id, UpdateAgencyDTO agencyDto)
    {
        var agency = await GetAgencyByIdAsync(id);

        if (agency == null)
            throw new KeyNotFoundException($"Agency with id {id} not found");
        
        mapper.Map(agencyDto, agency);
        
        await dbcontext.SaveChangesAsync();
        
    }
}