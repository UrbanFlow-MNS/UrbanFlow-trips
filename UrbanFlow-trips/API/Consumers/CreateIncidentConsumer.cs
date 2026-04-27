using MassTransit;
using UrbanFlow_trips.Application.Records;
using UrbanFlow_trips.DTO.Incident;
using UrbanFlow_trips.Infrastructure.Repository;

namespace UrbanFlow_trips.API.Consumers;

public class CreateIncidentConsumer(ILogger<CreateIncidentConsumer> logger, IIncidentRepository repo) :  IConsumer<NestJsMessage<CreateIncidentRecord>>
{

    public async Task Consume(ConsumeContext<NestJsMessage<CreateIncidentRecord>> context)
    {
        if (context.Message.Pattern != "incident.created")
            return;

        var incident = context.Message.Data;

        logger.LogInformation(
            "Incident reçu : Id={IncidentId}, Site={RouteId}, Priorité={Priority}",
            incident.IncidentId, incident.SiteId, incident.Priority);

        foreach (var routeId in incident.AffectedRouteIds)
        {
            CreateIncidentDto incidentDto = new CreateIncidentDto
            {
                RouteId = routeId,
                EstimateDuration = incident.EstimateDuration,
            };

            await repo.CreateIncidentAsync(incidentDto);
        }
    }
}