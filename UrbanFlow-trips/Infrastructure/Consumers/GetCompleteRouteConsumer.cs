using MassTransit;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Infrastructure.Consumers;

public class GetCompleteRouteConsumer : IConsumer<GetTripRequest>
{
    private readonly ITripRepository _tripRepository;

    public GetCompleteRouteConsumer(ITripRepository tripRepository)
    {
        tripRepository = _tripRepository;
    }
    
    public async Task Consume(ConsumeContext<GetTripRequest> context)
    {
        var trips = await _tripRepository.GetAllTripsAsync();
        await context.RespondAsync(trips);
    }
}