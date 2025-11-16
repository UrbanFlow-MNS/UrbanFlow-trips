using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Database;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TripsDbContext>();

        context.Database.EnsureCreated();

    }
    catch (Exception ex)
    {
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();




app.Run();