using MelodyMuse.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("test")]
        //用于测试，和test.TEST()对应
        public IActionResult GetNewData()
        {
            try
            {
                using (var context = new ModelContext())
                {
                    // 获取所有艺术家信息
                    var artists = context.Artists.ToList();

                    // 构建结果字符串
                    string result = "";
                    foreach (var artist in artists)
                    {
                        result += $"Artist Name: {artist.ArtistName} Artist Birthday: {artist.ArtistBirthday}\n";
                    }

                    // 返回请求成功，并包含艺术家信息
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
