using Prometheus;

namespace UrbanFlow_trips.Domain.Service;

public class PrometheusService
{
    public PrometheusService()
    {
        Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
        {
            { "app", "dotnet9-prometheus" }
        });
    }

    public async Task<string> GetMetricsAsync()
    {
        using var stream = new MemoryStream();
        await Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
        
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}