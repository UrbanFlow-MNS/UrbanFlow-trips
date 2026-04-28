using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Domain.Interfaces;

public interface IStopRepository
{
    Task CreateStopAsync(CreateStopDto stopDto);
    Task<List<GetStopDto>> GetAllStopsAsync();
    Task UpdateStopAsync(int id, UpdateStopDto stop);
    Task DeleteStopAsync(int id);
    Task<bool> StopExistsAsync(int id, CancellationToken cancellationToken = default);
}