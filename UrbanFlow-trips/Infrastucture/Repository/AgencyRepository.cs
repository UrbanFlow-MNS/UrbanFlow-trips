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
        var agency = mapper.Map<Agency>(agencyDto);
        await dbcontext.Agencies.AddAsync(agency);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<Agency?> GetAgencyByIdAsync(int id)
    {
        return await dbcontext.Agencies.FindAsync(id);
    }

    public async Task<List<GetAgencyDTO>> GetAllAgenciesAsync()
    {
        var Agency = await dbcontext.Agencies.ToListAsync();
        return mapper.Map<List<GetAgencyDTO>>(Agency);
    }
    
    public async Task UpdateAgencyAsync(int id, UpdateAgencyDTO agencyDto)
    {
        var agency = await GetAgencyByIdAsync(id);

        if (agency == null)
            throw new NullReferenceException("Agency not found");
        
        mapper.Map(agencyDto, agency);
        
        await dbcontext.SaveChangesAsync();
        
    }
}