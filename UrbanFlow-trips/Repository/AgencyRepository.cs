using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class AgencyRepository(TripsDbContext dbcontext)
{
    public async Task CreateAgencyAsync(CreateAgencyDTO agencyDto)
    {
        Agency agency = new Agency()
        {
            AgencyName = agencyDto.AgencyName,
            TimeZone = agencyDto.TimeZone
        };
        await dbcontext.Agencies.AddAsync(agency);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<Agency>> GetAllAgenciesAsync()
    {
        return await dbcontext.Agencies.ToListAsync();
    }
}