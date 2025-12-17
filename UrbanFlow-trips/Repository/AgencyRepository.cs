using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class AgencyRepository(TripsDbContext dbcontext, IMapper _mapper)
{
    public async Task CreateAgencyAsync(CreateAgencyDTO agencyDto)
    {
        var agency = _mapper.Map<Agency>(agencyDto);
        await dbcontext.Agencies.AddAsync(agency);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<GetAgencyDTO>> GetAllAgenciesAsync()
    {
        var Agency = await dbcontext.Agencies.ToListAsync();
        return _mapper.Map<List<GetAgencyDTO>>(Agency);
        
    }
}