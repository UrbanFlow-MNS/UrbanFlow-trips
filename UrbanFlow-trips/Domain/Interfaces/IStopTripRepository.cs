using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface IStopTripRepository
{
    Task CreateStopTripAsync(CreateStopTripDTO stopTripDto);
    Task UpdateStopTripAsync(int stopId, int tripId, UpdateStopTripDTO stopTripDto);
    Task<Stop_Trip?> GetStopTripByIdAsync(int stopId, int tripId);
}