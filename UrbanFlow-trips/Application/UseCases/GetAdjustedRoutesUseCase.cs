using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.Repository;


public class GetAdjustedRoutesUseCase(IRoutesRepository routesRepository, IIncidentRepository incidentRepository) : IGetAdjustedRoutesUseCase
{
    public async Task<List<GetCompleteRouteDto>> ExecuteAsync(RouteFilterDto filter)
    {
        var routes = await routesRepository.GetRoutesFilter(filter);

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