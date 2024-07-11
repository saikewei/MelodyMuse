using Microsoft.AspNetCore.Mvc;

namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/player")]
    public class MusicPlayerController : Controller
    {
        private readonly string _musicFilePath = @"./Resources/";

        // 通过歌曲ID获得歌曲的请求响应
        [HttpGet]
        [Route("{musicId}")]
        public IActionResult StreamMusic(string musicId)
        {
            try
            {
                var fileStream = System.IO.File.OpenRead(_musicFilePath + musicId + ".mp3");
                var contentType = "audio/mpeg";

                return File(fileStream, contentType);
            }
            catch (Exception ex) when (
                ex is FileNotFoundException ||
                ex is UnauthorizedAccessException ||
                ex is IOException)
            {
                // 文件未找到或无法访问时的处理逻辑
                Console.WriteLine($"文件打开失败：{ex.Message}");
                return NotFound(); // 返回 HTTP 404 响应
            }
        }
    }
}
