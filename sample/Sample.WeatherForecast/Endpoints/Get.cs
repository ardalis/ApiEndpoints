using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.WeatherForecast.Endpoints;

public class Get : EndpointBaseSync
  .WithoutRequest
  .WithActionResult<IEnumerable<WeatherForecast>>
{
  private static readonly string[] Summaries = new[]
  {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  private readonly ILogger<Get> _logger;

  public Get(ILogger<Get> logger)
  {
    _logger = logger;
  }

  [HttpGet("/WeatherForecast")]
  [SwaggerOperation(
    Summary = "Get weather forecast",
    OperationId = "WeatherForecast.Get",
    Tags = new[] { "WeatherForecast" })
  ]
  public override ActionResult<IEnumerable<WeatherForecast>> Handle()
  {
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                     {
                       Date = DateTime.Now.AddDays(index),
                       TemperatureC = Random.Shared.Next(-20, 55),
                       Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                     })
                     .ToArray();
  }
}
