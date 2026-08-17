using UrbanFlow_trips.Domain.Service;

namespace UrbanFlow_trips.MsTest.TestHelpers;

/// <summary>
/// PrometheusService pose des labels statiques sur le registre global de prometheus-net :
/// il ne doit être construit qu'une seule fois par processus de test.
/// </summary>
public static class SharedPrometheus
{
    public static readonly PrometheusService Instance = new();
}
