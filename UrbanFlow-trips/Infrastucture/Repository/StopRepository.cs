using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class StopRepository(TripsDbContext dbcontext, IMapper mapper) : IStopRepository
{
    public async Task<Stop_Times?> GetStopByIdAsync(int id)
    {
        return await dbcontext.Stops.FindAsync(id);
    }
    public async Task UpdateStopAsync(int id, UpdateStopDTO stop)
    {
        var stopToUpdate = await GetStopByIdAsync(id);
        mapper.Map(stop, stopToUpdate);
        await dbcontext.SaveChangesAsync();
    }
    public async Task CreateStopAsync(CreateStopDTO stopDto)
    {
        var stop = mapper.Map<Stop_Times>(stopDto);
        await dbcontext.Stops.AddAsync(stop);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<GetStopDTO>> GetAllStopsAsync()
    {
        
        var StopTimes = await dbcontext.Stops.ToListAsync();
        return mapper.Map<List<GetStopDTO>>(StopTimes);
    }
}