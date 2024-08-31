using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Configure;

namespace MelodyMuse.Server.Controllers
{

    [ApiController]
    [Route("api/usersub")]
    public class UserUploadController : Controller
    {
        private readonly IUploadSongService _uploadsongService;
        public UserUploadController(IUploadSongService songService)
        {
            _uploadsongService = songService;
        }

        [HttpPost("uploadsong")]
        [Authorize]
        public async Task<IActionResult> UploadSong([FromForm] SongUploadByUserModel songUploadByUserDto)
        {
            // 从请求头中获取 JWT 令牌//
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //如果没有令牌，返回未授权错误码401//
            if (token == null)
            {
                return Unauthorized();
            }

            // 解析 JWT 令牌 得到存储的信息ParsedToken:id,name,phone
            var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);
            //下面是输出测试查看是否正确//
            Console.WriteLine(parsedToken.UserID + " " + parsedToken.Username + " " + parsedToken.UserPhone);
            var userId = parsedToken.UserID;

            // 检查songUploadDto是否为null
            if (songUploadByUserDto == null)
            {
                return BadRequest("Song data is required.");
            }

            // 检查歌曲文件是否存在
            if (songUploadByUserDto.SongFile == null || songUploadByUserDto.SongFile.Length == 0)
            {
                return BadRequest("Song file is required.");
            }

            // 调用服务层方法上传歌曲
            var result = await _uploadsongService.UserUploadSongAsync(songUploadByUserDto, userId);
            if (result)
            {
                return Ok(new { message = "Song uploaded successfully." });
            }

            return BadRequest("Error uploading song.");
        }

    }
}
