using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface IStopTripRepository
{
    Task CreateStopTripAsync(CreateStopTripDTO stopTripDto);
    Task<List<Stop_Trip>> GetAllStopTripsAsync();
}