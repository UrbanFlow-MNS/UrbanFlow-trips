using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class TripRepository(TripsDbContext dbcontext, StopTripRepository stopTripRepository, IMapper mapper) : ITripRepository
{
    public async Task CreateTripAsync(CreateTripDTO tripDto)
    {
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

    public async Task<List<Trip>> GetAllTripsAsync()
    {
        return await dbcontext.Trips.ToListAsync();
    }
}