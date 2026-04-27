using MassTransit;
using UrbanFlow_trips.Application.Records;

namespace UrbanFlow_trips.API.Consumers;

public class CreateIncidentConsumer :  IConsumer<NestJsMessage<CreateIncidentRecord>>
{
    private readonly ILogger<CreateIncidentConsumer> _logger;

    public IncidentCreatedConsumer(ILogger<CreateIncidentConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NestJsMessage<CreateIncidentRecord>> context)
    {
        if (context.Message.Pattern != "incident.created")
            return;

        var incident = context.Message.Data;

        _logger.LogInformation(
            "Incident reçu : Id={IncidentId}, Site={SiteId}, Priorité={Priority}",
            incident.IncidentId, incident.SiteId, incident.Priority);
        
        

        await Task.CompletedTask;
    }
}