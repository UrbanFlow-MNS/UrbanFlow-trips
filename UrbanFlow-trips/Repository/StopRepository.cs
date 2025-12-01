using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class StopRepository(TripsDbContext dbcontext)
{
    public async Task CreateStopAsync(CreateStopDTO stopDto)
    {
        Stop_Times stop = new Stop_Times()
        {
            StopName = stopDto.StopName,
            StopLat = stopDto.StopLat,
            StopLong = stopDto.StopLong
        };
        
        await dbcontext.Stops.AddAsync(stop);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<Stop_Times>> GetAllStopsAsync()
    {
        return await dbcontext.Stops.ToListAsync();
    }
}