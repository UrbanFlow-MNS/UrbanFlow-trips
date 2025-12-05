using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class TripRepository(TripsDbContext dbcontext)
{
    public async Task CreateTripAsync(CreateTripDTO tripDto)
    {
        Trip trip = new Trip()
        {
            RouteId = tripDto.RouteId,
            ServiceId = tripDto.ServiceId,
            TripHeadsign = tripDto.TripHeadsign
        };
        
        await dbcontext.Trips.AddAsync(trip);
        await dbcontext.SaveChangesAsync();
        
    }

    public async Task<List<Trip>> GetAllTripsAsync()
    {
        return await dbcontext.Trips.ToListAsync();
    }
}