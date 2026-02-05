using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using UrbanFlow_trips;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Services;

public class TripService : Tripper.TripperBase
{
    private readonly IRoutesRepository _repository;

    public TripService(IRoutesRepository repository)
    {
        _repository = repository;
    }

    public override async Task<AllCompleteRoute> FindAll(Empty request, ServerCallContext context)
    {
        var routes = await _repository.GetAllCompleteRoutesAsync();
        
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
                var tripDetails = new TripDetails
                {
                    TripId = trip.TripId
                };

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

    public override async Task<CompleteRoute> FindById(RouteRequest request, ServerCallContext context)
    {
        var route = await _repository.GetCompleteRouteByIdAsync(request.Id);

        if (route == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Route {request.Id} not found"));

        var response = new CompleteRoute
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
                    Longitude = (double) stop.Longitude,
                    Latitude = (double) stop.Latitude,
                    ArrivalTime = stop.ArrivalTime.ToString() ?? "",
                    SequenceOrder = stop.SequenceOrder
                });
            }

            response.Trips.Add(tripDetails);
        }

        return response;
    }

    
}