using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class StopTripRepository(TripsDbContext dbcontext, IMapper _mapper)
{
    public async Task CreateStopTripAsync(CreateStopTripDTO stopTripDto)
    {
        
        Stop_Trip stopTrip = _mapper.Map<Stop_Trip>(stopTripDto);
        await dbcontext.StopTrips.AddAsync(stopTrip);
    }

    public async Task<List<Stop_Trip>> GetAllStopTripsAsync()
    {
        return await dbcontext.StopTrips.ToListAsync();
    }
}