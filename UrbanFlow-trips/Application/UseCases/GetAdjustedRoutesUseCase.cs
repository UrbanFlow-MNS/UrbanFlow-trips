using UrbanFlow_trips.Application.DTO.CompleteRoute;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.Application.UseCases;

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

            foreach (var stop in route.Trips.SelectMany(trip => trip.Stops))
            {
                stop.CurrentArrivalTime += delaySeconds;
                stop.Delay += delaySeconds;
                

            }
            
            
            
        }

        return routes;
    }
}