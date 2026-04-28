using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Exceptions;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class StopTripRepository(TripsDbContext dbcontext, IMapper mapper, IStopRepository stopRepository) : IStopTripRepository
{
    public async Task<Stop_Trip?> GetStopTripByIdAsync(int stopId, int tripId)
    {
        return await dbcontext.StopTrips.Where(x => x.StopId == stopId && x.TripId == tripId).FirstOrDefaultAsync();
    }
    public async Task CreateStopTripAsync(CreateStopTripDto stopTripDto)
    {
        ArgumentNullException.ThrowIfNull(stopTripDto);
        
        if (!await stopRepository.StopExistsAsync(stopTripDto.StopId))
            throw new NotFoundException($"Agency with id {stopTripDto.StopId} not found", 404);

        Stop_Trip stopTrip = mapper.Map<Stop_Trip>(stopTripDto);
        await dbcontext.StopTrips.AddAsync(stopTrip);
    }

    public async Task<List<Stop_Trip>> GetAllStopTripsAsync()
    {
        return await dbcontext.StopTrips.AsNoTracking().ToListAsync();
    }

    public async Task DeleteStopTripAsync(int stopId, int tripId)
    {
        var stopTrip = await GetStopTripByIdAsync(stopId, tripId);
        
        if (stopTrip == null)
            throw new KeyNotFoundException($"StopTrip (StopId={stopId}, TripId={tripId}) not found");
        
        dbcontext.StopTrips.Remove(stopTrip);
        await dbcontext.SaveChangesAsync();
    }
    
    
    public async Task UpdateStopTripAsync(int stopId, int tripId, UpdateStopTripDto stopTripDto)
    {
        var stopTrip = await GetStopTripByIdAsync(stopId, tripId);
        
        if (stopTrip == null)
            throw new KeyNotFoundException($"StopTrip (StopId={stopId}, TripId={tripId}) not found");
        
        mapper.Map(stopTripDto, stopTrip);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<bool> StopTripExistsAsync(int stopId, int tripId, CancellationToken cancellationToken = default)
    {
        return await dbcontext.StopTrips.AnyAsync(x => x.StopId == stopId && x.TripId == tripId, cancellationToken);
    }
    
}