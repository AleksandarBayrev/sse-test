using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SSETests.Controllers
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
    [ApiController]
    public class SSEController : Controller
    {

        private readonly string[] summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        [HttpGet("/weatherforecasts")]
        public async Task GetData(CancellationToken cancellationToken)
        {
            HttpContext.Response.ContentType = "text/event-stream";

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancelling request at " + DateTime.Now.ToLongDateString());
                    break;
                }

                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();

                await HttpContext.Response.WriteAsync("event: server_logs\n");
                await HttpContext.Response.WriteAsync($"data: {JsonSerializer.Serialize(forecast)}\n\n");
                await HttpContext.Response.Body.FlushAsync();
                await Task.Delay(0);
            }
        }
    }
}