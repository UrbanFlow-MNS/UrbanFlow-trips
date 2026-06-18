using System.Text.Json;
using MassTransit;
using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Application.Records;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.API.Consumers;
public class IncidentConsumer(ILogger<IncidentConsumer> logger, IIncidentRepository repo) : IConsumer<object> 
{
    public async Task Consume(ConsumeContext<object> context)
    {
        var rawJson = System.Text.Encoding.UTF8.GetString(
            context.ReceiveContext.GetBody());

        var wrapper = JsonSerializer.Deserialize<NestJsWrapper>(rawJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (wrapper?.Pattern is null || wrapper.Data is null)
        {
            logger.LogWarning("Message ignoré : pattern ou data invalide");
            return;
        }

        switch (wrapper.Pattern)
        {
            case "incident.created":
                await HandleCreated(wrapper.Data, logger, repo);
                break;

            case "incident.closed":
                await HandleRemoved(wrapper.Data, logger, repo);
                break;

            default:
                logger.LogWarning("Pattern inconnu : {Pattern}", wrapper.Pattern);
                break;
        }
    }

    private static async Task HandleCreated(CreateIncidentRecord incident, ILogger logger, IIncidentRepository repo)
    {
        logger.LogInformation(
            "Incident créé : Id={IncidentId}, Site={SiteId}, Priorité={Priority}",
            incident.IncidentId, incident.SiteId, incident.Priority);

        foreach (var routeId in incident.AffectedRouteIds)
        {
            await repo.CreateIncidentAsync(new CreateIncidentDto
            {
                IncidentId = incident.IncidentId,
                RouteId = routeId,
                EstimateDuration = incident.EstimateDuration,
            });
        }
    }

    private static async Task HandleRemoved(CreateIncidentRecord incident, ILogger logger, IIncidentRepository repo)
    {
        logger.LogInformation(
            "Incident fermé : Id={IncidentId}", incident.IncidentId);

        await repo.DeleteIncidentAsync(incident.IncidentId);
    }
}