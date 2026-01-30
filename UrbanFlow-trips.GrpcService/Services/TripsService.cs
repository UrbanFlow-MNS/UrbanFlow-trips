using Grpc.Core;
using UrbanFlow_trips.Models;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.GrpcService.Services;

public class TripsService(ITripRepository tripRepository) : TripService.TripServiceBase
{
    
    public override async Task<TripsList> FindAll(Empty request, ServerCallContext context)
    {
        var trips = await tripRepository.GetAllTripsAsync();
                    
        // Créer la réponse gRPC
        var response = new TripsList();
        
        // Convertir vos entités Trip en TripData (messages protobuf)
        foreach (var trip in trips)
        {
            response.Trips.Add(new TripData
            {
                Id = trip.RouteId
            });
        }
        
        return response;
    }
}