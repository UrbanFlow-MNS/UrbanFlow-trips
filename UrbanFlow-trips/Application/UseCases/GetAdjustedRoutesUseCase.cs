using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.Repository;


public class GetAdjustedRoutesUseCase(IRoutesRepository routesRepository, IIncidentRepository incidentRepository) : IGetAdjustedRoutesUseCase
{
    public async Task<List<GetCompleteRouteDto>> ExecuteAsync(int? agencyId = null)
    {
        var routes = agencyId.HasValue
            ? await routesRepository.GetCompleteRouteByIdAsync(agencyId.Value)
            : await routesRepository.GetAllCompleteRoutesAsync();

        foreach (var route in routes)
        {
            var delayMinutes = await incidentRepository.EstimateMinutesLateByRouteId(route.RouteId);
            if (delayMinutes is null) continue;

            var delaySeconds = delayMinutes.Value * 60;

            foreach (var trip in route.Trips)
            foreach (var stop in trip.Stops)
                stop.ArrivalTime += delaySeconds;  
        }

        return routes;
    }
}