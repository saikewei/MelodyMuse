using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // 如果是 ASP.NET Core
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost("follow/increment-fans")]
        public async Task<IActionResult> IncrementFansNum([FromQuery] string artistId)
        {
            var result = await _artistService.IncrementArtistFansNumAsync(artistId);

            if (result)
            {
                return Ok(new { Message = "艺术家粉丝添加成功" });
            }
            else
            {
                return BadRequest(new { Message = "添加失败" });
            }
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await _artistService.GetAllArtistsAsync();
            return Ok(artists);
        }
        [Authorize]
        [HttpGet("user/{userId}/followed")]
        public async Task<IActionResult> GetFollowedArtistsByUserId(string userId)
        {
            var artists = await _artistService.GetArtistsByUserIdAsync(userId);
            return Ok(artists);
        }
        [Authorize]
        [HttpGet("{artistId}/fans-count")]
        public async Task<IActionResult> GetArtistFansCount(string artistId)
        {
            var fansCount = await _artistService.GetArtistFansCountAsync(artistId);
            return Ok(new { ArtistId = artistId, FansCount = fansCount });
        }
    }
}