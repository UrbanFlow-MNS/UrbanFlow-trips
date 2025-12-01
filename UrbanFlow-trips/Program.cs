using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<AgencyRepository>();
builder.Services.AddScoped<RoutesRepository>();
builder.Services.AddScoped<RouteTypeRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

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