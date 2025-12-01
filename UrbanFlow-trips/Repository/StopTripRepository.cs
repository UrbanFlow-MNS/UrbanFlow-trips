using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class StopTripRepository(TripsDbContext dbcontext)
{
    public async Task CreateStopTripAsync(CreateStopTripDTO stopTripDto)
    {
        Stop_Trip stopTrip = new Stop_Trip()
        {
            TripId = stopTripDto.TripId,
            StopId = stopTripDto.StopId,
            StopSequence = stopTripDto.StopSequence,
            ArrivalTime = stopTripDto.ArrivalTime,
            DepartureTime = stopTripDto.DepartureTime
        };
        
        await dbcontext.StopTrips.AddAsync(stopTrip);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<Stop_Trip>> GetAllStopTripsAsync()
    {
        return await dbcontext.StopTrips.ToListAsync();
    }
}