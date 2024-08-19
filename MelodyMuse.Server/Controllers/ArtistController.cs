using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace MelodyMuse.Server.Controllers
{
[ApiController]
    [Route("api/artist")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("{artistId}")]
        public async Task<IActionResult> GetArtistById(string artistId)
        {
            var artist = await _artistService.GetArtistByIdAsync(artistId);

            if (artist == null)
            {
                return NotFound(new { message = "艺术家未找到。" });
            }

            return Ok(artist);
        }

        [HttpGet("{artistId}/songs")]
        public async Task<IActionResult> GetSongsByArtistId(string artistId)
        {
            var songs = await _artistService.GetSongsByArtistIdAsync(artistId);
            
            if (songs == null || songs.Count == 0)
            {
                return NotFound(new { message = "该歌手未发布歌曲" });
            }

            return Ok(songs);
        }

        // 用户关注艺术家
        [HttpPost("follow")]
        public async Task<IActionResult> FollowArtist([FromBody] FollowArtistRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.ArtistId))
            {
                return BadRequest(new { message = "请求为空" });
            }

            var result = await _artistService.FollowArtistAsync(request.UserId, request.ArtistId);
            if (result)
            {
                return Ok(new { message = "关注歌手成功" });
            }

            return BadRequest(new { message = "关注歌手失败" });
        }

        [HttpPost("unfollow")]
    public async Task<IActionResult> UnfollowArtist([FromBody] FollowArtistRequest model)
    {
        var result = await _artistService.UnfollowArtistAsync(model.UserId, model.ArtistId);

        if (result)
        {
            return Ok(new { Message = "取消关注成功" });
        }
        else
        {
            return BadRequest(new { Message = "取消关注失败" });
        }
    }

    }
}