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
                //数据库查询语句
                string sql = "select * from c##main_schema.artist";

                //调用函数查询得到结果表
                List<Dictionary<string, object>> table = DatabaseOperation.Select(sql);
                string result = "";

                //循环读取每一行
                foreach (var item in table)
                {
                    //循环读取每一列
                    foreach (string key in item.Keys)
                    {
                        result += key + ":" + item[key] + " ";
                    }
                    result += "\n";
                }

                // 返回请求
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
