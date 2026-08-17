using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using UrbanFlow_trips.Domain.Service;
using UrbanFlow_trips.Infrastructure.Database;
using UrbanFlow_trips.MsTest.TestHelpers;

namespace UrbanFlow_trips.MsTest.Functional;

/// <summary>
/// Factory principale : base InMemory + transport MassTransit in-memory (test harness).
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ReplaceDbContext(services, options => options
                .UseInMemoryDatabase(_dbName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            services.AddMassTransitTestHarness();
            ReplacePrometheus(services);
        });
    }

    internal static void ReplaceDbContext(IServiceCollection services, Action<DbContextOptionsBuilder> configure)
    {
        var toRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<TripsDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(TripsDbContext) ||
                // EF Core 9 enregistre la configuration Npgsql via IDbContextOptionsConfiguration<T>
                d.ServiceType == typeof(IDbContextOptionsConfiguration<TripsDbContext>))
            .ToList();
        foreach (var descriptor in toRemove)
            services.Remove(descriptor);

        services.AddDbContext<TripsDbContext>(configure);
    }

    internal static void ReplacePrometheus(IServiceCollection services)
    {
        // PrometheusService pose des labels statiques globaux : une seule instance par processus de test
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(PrometheusService));
        if (descriptor != null)
            services.Remove(descriptor);
        services.AddSingleton(SharedPrometheus.Instance);
    }
}

/// <summary>
/// Factory en environnement Production pour couvrir la branche hors-Development de Program.cs.
/// </summary>
public class ProductionWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        builder.ConfigureTestServices(services =>
        {
            CustomWebApplicationFactory.ReplaceDbContext(services, options => options
                .UseInMemoryDatabase(_dbName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            services.AddMassTransitTestHarness();
            CustomWebApplicationFactory.ReplacePrometheus(services);
        });
    }
}

/// <summary>
/// Factory avec Sqlite relationnel : Program.cs appelle context.Database.Migrate() au démarrage.
/// Les migrations générées sont spécifiques à Npgsql, donc IMigrator est remplacé par un no-op
/// pour couvrir le chemin de succès de la migration de façon déterministe.
/// </summary>
public class SqliteWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();
        builder.ConfigureTestServices(services =>
        {
            CustomWebApplicationFactory.ReplaceDbContext(services, options => options
                .UseSqlite(_connection)
                .ReplaceService<IMigrator, NoOpMigrator>());
            services.AddMassTransitTestHarness();
            CustomWebApplicationFactory.ReplacePrometheus(services);
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            _connection.Dispose();
    }
}

public class NoOpMigrator : IMigrator
{
    public void Migrate(string? targetMigration = null)
    {
    }

    public Task MigrateAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public string GenerateScript(string? fromMigration = null, string? toMigration = null,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        => string.Empty;

    public bool HasPendingModelChanges() => false;
}

/// <summary>
/// Factory qui conserve le transport RabbitMQ réel (sans test harness) : la configuration
/// du bus (UsingRabbitMq) s'exécute au démarrage, sans connexion bloquante au broker.
/// </summary>
public class RabbitMqTransportWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            CustomWebApplicationFactory.ReplaceDbContext(services, options => options
                .UseInMemoryDatabase(_dbName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            CustomWebApplicationFactory.ReplacePrometheus(services);
        });
    }
}
