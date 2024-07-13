using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;


namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/player")]
    public class MusicPlayerController : Controller
    {
        private readonly IMusicPlayerService _musicService;
        private readonly string _songFilePath = @"./Resources/";

        public MusicPlayerController(IMusicPlayerService musicService)
        {
            _musicService = musicService;
        }

        // 通过歌曲ID获得歌曲的请求响应
        [HttpGet]
        [Route("file/{songId}")]
        public async Task<IActionResult> StreamMusic(string songId)
        {
            try
            {
                var fileStream = new FileStream(_songFilePath + songId + ".mp3", FileMode.Open, FileAccess.Read);
                var response = new FileStreamResult(fileStream, "audio/mpeg")
                {
                    FileDownloadName = songId + ".mp3"
                };

                // 支持范围请求
                response.EnableRangeProcessing = true;

                return response;
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

        [HttpGet]
        [Route("{songId}")]
        public async Task<IActionResult> GetMusicInfo(string songId)
        {
             var song = await _musicService.GetSongBySongId(songId);

             if (song == null)
             {
                 // 歌曲ID不存在时的处理逻辑
                 return NotFound("歌曲ID不存在");
             }

            var successResponse = new
            {
                
            };
            
            return Ok(successResponse);
        }
    }
}
