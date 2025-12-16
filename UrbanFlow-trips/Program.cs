using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Mapping;
using UrbanFlow_trips.Options;
using UrbanFlow_trips.Repository;
using UrbanFlow_trips.Service;


var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<AgencyRepository>();
builder.Services.AddScoped<RoutesRepository>();
builder.Services.AddScoped<RouteTypeRepository>();
builder.Services.AddScoped<StopRepository>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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


/*
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();  
}
*/

app.UseHttpsRedirection();  
app.UseAuthorization();
app.MapControllers();

app.Run();