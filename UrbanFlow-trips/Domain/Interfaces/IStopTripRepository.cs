using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface IStopTripRepository
{
    Task CreateStopTripAsync(CreateStopTripDto stopTripDto);
    Task UpdateStopTripAsync(int stopId, int tripId, UpdateStopTripDto stopTripDto);
    Task<Stop_Trip?> GetStopTripByIdAsync(int stopId, int tripId);
    Task DeleteStopTripAsync(int stopId, int tripId);
}