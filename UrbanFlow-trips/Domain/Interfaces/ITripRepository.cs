using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public interface ITripRepository
{
    Task CreateTripAsync(CreateTripDTO tripDto);
    Task<List<Trip>> GetAllTripsAsync();
    Task UpdateTripService(int id, UpdateTripServiceDTO serviceId);
    
    
}