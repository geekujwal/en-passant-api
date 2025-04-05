using System.Threading.Tasks;
using Edison.Producer;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    
    private readonly MessageProducer _messageProducer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, MessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    [HttpGet]
    public async Task<OkResult>  Get()
    {
        await _messageProducer.SendMessage();
        return Ok();
    }
}
