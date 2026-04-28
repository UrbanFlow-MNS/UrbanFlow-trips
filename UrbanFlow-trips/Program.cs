using System.Net.Mime;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Options;
using UrbanFlow_trips.Repository;
using UrbanFlow_trips.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using MassTransit;
using UrbanFlow_trips;
using UrbanFlow_trips.API.Consumers;
using UrbanFlow_trips.Application.Mapping;
using UrbanFlow_trips.Application.Validators;
using UrbanFlow_trips.Domain.Interfaces;
using UrbanFlow_trips.Infrastructure.Repository;
using UrbanFlow_trips.Infrastucture.Repository;
using UrbanFlow_trips.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddOpenApi();

// Configuration des mappers
builder.Services.AddAutoMapper(
    cfg => {}, 
    typeof(RouteMappingProfile)
);

// Configuration RabbitMQ
builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMq"));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Configuration Dbcontext
builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseNpgsql(connectionString));

// Injection de dépendances des repositories
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IRoutesRepository, RoutesRepository>();
builder.Services.AddScoped<IStopRepository, StopRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IStopTripRepository, StopTripRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IGetAdjustedRoutesUseCase, GetAdjustedRoutesUseCase>();
builder.Services.AddSingleton<PrometheusService>();


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRouteDtoValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGrpcClient<Vehicler.VehiclerClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:TransportManagement"]);
})
.ConfigureChannel(options =>
{
    options.HttpHandler = new SocketsHttpHandler
    {
        KeepAlivePingDelay = TimeSpan.FromSeconds(10),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(5),
        KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always,
        EnableMultipleHttp2Connections = true
    };
});

builder.Services.AddScoped<VehicleService>();

// Config MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PostLogsConsumer>();
    x.AddConsumer<CreateIncidentConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("urban");
            h.Password("atSYR32V");
        });
        
        
        cfg.ReceiveEndpoint("LOGS_QUEUE", e =>
        {
            e.Durable = false;
            e.ConfigureConsumer<PostLogsConsumer>(context);
        });
        
        
        cfg.ReceiveEndpoint("INCIDENTS_QUEUE", e =>
        {
            e.Durable = true;
            e.DefaultContentType = new ContentType("application/json");
            e.UseRawJsonDeserializer(RawSerializerOptions.AnyMessageType);
            e.ConfigureConsumer<CreateIncidentConsumer>(context);
        });

        Console.WriteLine("ReceiveEndpoint INCIDENTS_QUEUE configuré");

    });
});

var app = builder.Build();


// Update de la ddb à partir de la dernière migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TripsDbContext>();
        
        context.Database.Migrate();
        
        Console.WriteLine("Database migrations OK.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration NOT OK: {ex.Message}");
    }
}



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(options =>
    {
        options.Title = "UrbanFlow Trips API";
        options.Theme = ScalarTheme.Moon;
    });
}

app.MapGrpcService<GreeterService>();
app.MapGrpcService<TripService>();
// juste pour tester le greeter
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


//app.UseHttpsRedirection();  
app.UseAuthorization();
app.MapControllers();

app.Run();