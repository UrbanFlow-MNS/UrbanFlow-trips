using System.Text;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using UrbanFlow_trips.Database;
using UrbanFlow_trips.Repository;


ConnectionFactory factory = new ConnectionFactory(){HostName = "rabbitmq", UserName = "user", Password = "password"};
using IConnection? connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

const string message = "Hello World!";
byte[] body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
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