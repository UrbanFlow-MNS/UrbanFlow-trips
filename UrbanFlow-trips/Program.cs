using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Options;
using UrbanFlow_trips.Repository;
using UrbanFlow_trips.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using UrbanFlow_trips;
using UrbanFlow_trips.Application.Mapping;
using UrbanFlow_trips.Application.Validators;
using UrbanFlow_trips.Infrastucture.Messaging;
using UrbanFlow_trips.Infrastucture.Repository;


var builder = WebApplication.CreateBuilder(args);


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
builder.Services.AddScoped<IAgencyRepository, AgencyRepository>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IRoutesRepository, RoutesRepository>();
builder.Services.AddScoped<IRouteTypeRepository, RouteTypeRepository>();
builder.Services.AddScoped<IStopRepository, StopRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IStopTripRepository, StopTripRepository>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAgencyDtoValidator>();

builder.Services.AddEndpointsApiExplorer();


// Configuration de MassTransit en GRPC et potentiellement RabbitMQ dans le futur si nécessaire

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


app.UseHttpsRedirection();  
app.UseAuthorization();
app.MapControllers();

app.Run();