public interface IPrometheusService
{
    Task<string> GetMetricsAsync();
}