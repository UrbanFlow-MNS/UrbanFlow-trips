namespace UrbanFlow_trips.MsTest.Functional;

[TestClass]
public class ProgramStartupTests
{
    [TestMethod]
    public async Task Application_StartsInProductionEnvironment()
    {
        using var factory = new ProductionWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task Application_StartsWithRelationalDatabase_AndRunsMigrations()
    {
        using var factory = new SqliteWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task Application_StartsWithRealRabbitMqTransportConfiguration()
    {
        using var factory = new RabbitMqTransportWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task OpenApi_IsExposedInDevelopment()
    {
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json");

        response.EnsureSuccessStatusCode();
    }
}
