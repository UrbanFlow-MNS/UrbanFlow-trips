using System.Net.Http.Json;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using UrbanFlow_trips.DTO;

class Program
{
    
    static async Task Main(string[] args)
    {
        // Foutre ça dans une classe à l'occasion, c'était juste pour le test
        var baseUrl = "http://localhost:5001/";
        var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

        CreateAgencyDto agency = new CreateAgencyDto()
        {
            AgencyName = "test",
            TimeZone = "test"
        };
        
        
        var scenario = Scenario.Create("get-complete-route", async context =>
            {
                var request = Http.CreateRequest("GET", "api/Routes/getDetails/2")
                    .WithHeader("Content-Type", "application/json");

                var response = await Http.Send(httpClient, request);
                return response;
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.RampingInject(
                    rate: 50,
                    interval: TimeSpan.FromSeconds(10),
                    during: TimeSpan.FromSeconds(30)
                )
                /*
                Simulation.Inject(
                    rate: 50,
                    interval: TimeSpan.FromSeconds(10),
                    during: TimeSpan.FromMinutes(1)
                ),
                Simulation.Inject(
                    rate: 50,
                    interval: TimeSpan.FromSeconds(10),
                    during: TimeSpan.FromSeconds(30))*/
            );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFormats(ReportFormat.Txt, ReportFormat.Md)
            .Run();
    }
}