using MassTransit;
using UrbanFlow_trips.Application.Records;

namespace UrbanFlow_trips.API.Consumers;

public class CreateIncidentConsumer(ILogger logger) :  IConsumer<NestJsMessage<CreateIncidentRecord>>
{

    public async Task Consume(ConsumeContext<NestJsMessage<CreateIncidentRecord>> context)
    {
        if (context.Message.Pattern != "incident.created")
            return;

        var incident = context.Message.Data;

        logger.LogInformation(
            "Incident reçu : Id={IncidentId}, Site={SiteId}, Priorité={Priority}",
            incident.IncidentId, incident.SiteId, incident.Priority);

        
        await Task.CompletedTask;
    }
}