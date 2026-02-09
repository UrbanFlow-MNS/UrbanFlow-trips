using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Infrastucture.Repository;

public class TripRepository(TripsDbContext dbcontext, IStopTripRepository stopTripRepository, IMapper mapper) : ITripRepository
{
    public async Task<Trip?> GetTripByIdAsync(int id)
    {
        return await dbcontext.Trips.FindAsync(id);
    }

    public async Task UpdateTripService(int id, UpdateTripServiceDto serviceDto)
    {
        ArgumentNullException.ThrowIfNull(serviceDto);

        var trip = await GetTripByIdAsync(id);
        if (trip == null)
            throw new KeyNotFoundException($"Trip with id {id} not found");
        trip.ServiceId = serviceDto.ServiceId;
        await dbcontext.SaveChangesAsync();
    }
    
    
    public async Task CreateTripAsync(CreateTripDto tripDto)
    {
        ArgumentNullException.ThrowIfNull(tripDto);

        await using var transaction = await dbcontext.Database.BeginTransactionAsync();

        try
        {
            Trip trip = mapper.Map<Trip>(tripDto);
            await dbcontext.Trips.AddAsync(trip);
            await dbcontext.SaveChangesAsync();

            int tripId = trip.TripId;
            int sequence = 1;

            foreach (var stopTripDto in tripDto.CreateStopTrips)
            {
                stopTripDto.TripId = tripId;
                stopTripDto.StopSequence = sequence;
                if (stopTripDto.DepartureTime == null)
                    stopTripDto.DepartureTime = stopTripDto.ArrivalTime;

                await stopTripRepository.CreateStopTripAsync(stopTripDto);
                sequence++;
            }
            
            await dbcontext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception("Error creating trip" + e.Message );
        }
    }
    
    public async Task DeleteTripAsync(int id)
    {
        var trip = await GetTripByIdAsync(id);
        
        if (trip == null)
            throw new KeyNotFoundException($"Trip with id {id} not found");
        
        dbcontext.Trips.Remove(trip);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<Trip>> GetAllTripsAsync()
    {
        return await dbcontext.Trips.ToListAsync();
    }
    
    public async Task<bool> TripExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbcontext.Trips.AnyAsync(x => x.TripId == id, cancellationToken);
    }
}