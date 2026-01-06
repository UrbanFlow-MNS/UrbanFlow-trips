using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface IStopRepository
{
    Task CreateStopAsync(CreateStopDTO stopDto);
    Task<List<GetStopDTO>> GetAllStopsAsync();
    Task UpdateStopAsync(int id, UpdateStopDTO stop);
}