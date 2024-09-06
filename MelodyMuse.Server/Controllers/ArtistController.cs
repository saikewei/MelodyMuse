using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;
namespace MelodyMuse.Server.Controllers
{
[ApiController]
    [Authorize]
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

        // 用户关注艺术家
        [Authorize]
        [HttpPost("follow")]
        public async Task<IActionResult> FollowArtist([FromBody] FollowArtistRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ArtistId))
            {
                return BadRequest(new { message = "请求为空" });
            }

            // 从请求头中获取 JWT 令牌
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // 如果没有令牌，返回未授权错误码401
            if (token == null)
            {
                return Unauthorized(new { msg = "未提供令牌" });
            }

            // 解析 JWT 令牌，得到存储的信息
            var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

            // 检查解析结果是否为空
            if (parsedToken == null)
            {
                return Unauthorized(new { msg = "令牌无效" });
            }

            var result = await _artistService.FollowArtistAsync(parsedToken.UserID, request.ArtistId);

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
            if (model == null || string.IsNullOrEmpty(model.ArtistId))
            {
                return BadRequest(new { message = "请求为空" });
            }

            // 从请求头中获取 JWT 令牌
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // 如果没有令牌，返回未授权错误码401
            if (token == null)
            {
                return Unauthorized(new { msg = "未提供令牌" });
            }

            // 解析 JWT 令牌，得到存储的信息
            var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

            // 检查解析结果是否为空
            if (parsedToken == null)
            {
                return Unauthorized(new { msg = "令牌无效" });
            }

            var result = await _artistService.UnfollowArtistAsync(parsedToken.UserID, model.ArtistId);

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
        public async Task<IActionResult> IncrementFansNum([FromQuery]string artistId)
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
        [HttpGet("FollowStatus/{artistId}")]
        public async Task<IActionResult> IsFollowArtist(string artistId)
        {
            // 从请求头中获取 JWT 令牌
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // 如果没有令牌，返回未授权错误码401
            if (token == null)
            {
                return Unauthorized(new { msg = "未提供令牌" });
            }

            // 解析 JWT 令牌，得到存储的信息
            var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

            // 检查解析结果是否为空
            if (parsedToken == null)
            {
                return Unauthorized(new { msg = "令牌无效" });
            }

            var result = await _artistService.IsFollowingArtistAsync(parsedToken.UserID, artistId);

            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
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
        [HttpGet("user/followed")]
        public async Task<IActionResult> GetFollowedArtists()
        {
            // 从请求头中获取 JWT 令牌
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // 如果没有令牌，返回未授权错误码401
            if (token == null)
            {
                return Unauthorized(new { msg = "未提供令牌" });
            }

            // 解析 JWT 令牌，得到存储的信息
            var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

            // 检查解析结果是否为空
            if (parsedToken == null)
            {
                return Unauthorized(new { msg = "令牌无效" });
            }

            var artists = await _artistService.GetArtistsByUserIdAsync(parsedToken.UserID);
            return Ok(artists);
        }
    }
}