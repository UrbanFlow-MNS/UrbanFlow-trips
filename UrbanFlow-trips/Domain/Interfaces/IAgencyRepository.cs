using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Repository;

public interface IAgencyRepository
{
    Task CreateAgencyAsync(CreateAgencyDTO agencyDto);
    Task<List<GetAgencyDTO>> GetAllAgenciesAsync();
}