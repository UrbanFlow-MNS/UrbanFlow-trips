using System.Text.Json;
using MassTransit;
using UrbanFlow_trips.Application.Records;
using UrbanFlow_trips.DTO.Incident;
using UrbanFlow_trips.Infrastructure.Repository;

public class CreateIncidentConsumer(ILogger<CreateIncidentConsumer> logger, IIncidentRepository repo)
    : IConsumer<CreateIncidentRecord>
{
    public async Task Consume(ConsumeContext<CreateIncidentRecord> context)
    {
        Console.WriteLine(context.Headers.ToList().ToString());
        Console.WriteLine(context.Message);
        Console.WriteLine(context.SerializerContext);


        var rawBytes = context.ReceiveContext.GetBody();
        var rawJson = System.Text.Encoding.UTF8.GetString(rawBytes);
        Console.WriteLine(rawJson);

        var wrapper = JsonSerializer.Deserialize<NestJsWrapper>(rawJson, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (wrapper?.Pattern != "incident.created" || wrapper.Data is null)
        {
            logger.LogWarning("Message ignoré : pattern ou data invalide");
            return;
        }

        var incident = wrapper.Data;

        logger.LogInformation(
            "Incident reçu : Id={IncidentId}, Site={SiteId}, Priorité={Priority}",
            incident.IncidentId, incident.SiteId, incident.Priority);

        foreach (var routeId in incident.AffectedRouteIds)
        {
            await repo.CreateIncidentAsync(new CreateIncidentDto
            {
                RouteId = routeId,
                EstimateDuration = incident.EstimateDuration,
            });
        }
    }
}