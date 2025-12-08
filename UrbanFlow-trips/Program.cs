using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;


ConnectionFactory factory = new ConnectionFactory(){HostName = "rabbitmq", UserName = "user", Password = "password"};
using IConnection? connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "LOGS_QUEUE_IN", durable: false, exclusive: false, autoDelete: false, arguments: null);

var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

var message = new
{
    pattern = "logs_created",
    data = new LogMessage
    {
        MicroserviceName = "test trips",
        CodeOfEvent = 200,
        Event = "Log created"
    }
};



var json = JsonSerializer.Serialize(message, option);
byte[] body = Encoding.UTF8.GetBytes(json);


await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "LOGS_QUEUE_IN", body: body);
Console.WriteLine(" [x] Sent {0}", message);


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