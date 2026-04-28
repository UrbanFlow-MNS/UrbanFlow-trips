using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using UrbanFlow_trips;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Services;

public class TripService(IGetAdjustedRoutesUseCase useCase) : Tripper.TripperBase
{

    public override async Task<AllCompleteRoute> FindAll(Empty request, ServerCallContext context)
    {
        var routes = await useCase.ExecuteAsync(new RouteFilterDto());
        return MapToResponse(routes);

    }
    

    public override async Task<AllCompleteRoute> FindById(RouteRequest request, ServerCallContext context)
    {
        var routes = await useCase.ExecuteAsync(new RouteFilterDto { RouteId = request.Id });
        return MapToResponse(routes);
    }
    
    private static AllCompleteRoute MapToResponse(List<GetCompleteRouteDto> routes)
    {
        var response = new AllCompleteRoute();

        foreach (var route in routes)
        {
            var completeRoute = new CompleteRoute
            {
                RouteId = route.RouteId,
                RouteShortName = route.RouteShortName ?? "",
                RouteLongName = route.RouteLongName ?? "",
                RouteTypeName = route.RouteTypeName ?? "",
            };

            foreach (var trip in route.Trips)
            {
                var tripDetails = new TripDetails { TripId = trip.TripId };

                foreach (var stop in trip.Stops)
                {
                    tripDetails.Stops.Add(new StopDetails
                    {
                        StopId = stop.StopId,
                        StopName = stop.StopName ?? "",
                        Longitude = (double)stop.Longitude,
                        Latitude = (double)stop.Latitude,
                        ArrivalTime = stop.ArrivalTime.ToString() ?? "",
                        SequenceOrder = stop.SequenceOrder
                    });
                }

                completeRoute.Trips.Add(tripDetails);
            }

            response.Routes.Add(completeRoute);
        }

        return response;
    }
    
}