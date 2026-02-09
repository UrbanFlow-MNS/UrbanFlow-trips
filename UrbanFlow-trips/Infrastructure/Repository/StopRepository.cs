using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Exceptions;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Repository;

public class StopRepository(TripsDbContext dbcontext, IMapper mapper, IAgencyRepository agencyRepository) : IStopRepository
{
    private async Task<Stop_Times?> GetStopByIdAsync(int id)
    {
        return await dbcontext.Stops.FindAsync(id);
    }
    public async Task UpdateStopAsync(int id, UpdateStopDto stop)
    {
        ArgumentNullException.ThrowIfNull(stop);

        var stopToUpdate = await GetStopByIdAsync(id);
        
        if (stopToUpdate == null)
            throw new KeyNotFoundException($"Stop with id {id} not found");
        
        mapper.Map(stop, stopToUpdate);
        await dbcontext.SaveChangesAsync();
    }
    public async Task CreateStopAsync(CreateStopDto stopDto)
    {
        ArgumentNullException.ThrowIfNull(stopDto);
        
        if (!await agencyRepository.AgencyExistsAsync(stopDto.AgencyId))
            throw new NotFoundException($"Agency with id {stopDto.AgencyId} not found", 404);

        var stop = mapper.Map<Stop_Times>(stopDto);
        await dbcontext.Stops.AddAsync(stop);
        await dbcontext.SaveChangesAsync();
    }

    public async Task DeleteStopAsync(int id)
    {
        var stop = await GetStopByIdAsync(id);
        
        if (stop == null)
            throw new KeyNotFoundException($"Stop with id {id} not found");
        
        dbcontext.Stops.Remove(stop);
        await dbcontext.SaveChangesAsync();
    }

    public async Task<List<GetStopDto>> GetAllStopsAsync()
    {
        var stopTimes = await dbcontext.Stops.AsNoTracking().ToListAsync();
        return mapper.Map<List<GetStopDto>>(stopTimes);
    }

    public async Task<bool> StopExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbcontext.Stops.AnyAsync(x => x.StopId == id, cancellationToken);
    }
}