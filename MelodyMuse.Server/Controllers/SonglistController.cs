using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Configure;


namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/songlist")]
    public class SonglistController : Controller
    {
        private readonly ISonglistService _songlistService;

        public SonglistController(ISonglistService songlistService)
        {
            _songlistService = songlistService;
        }

        // 获取某用户的所有歌单及其包含的歌曲数量
        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetUserSonglists()
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }

            var songlists = await _songlistService.GetUserSonglistsAsync(userId);
            if (songlists == null || !songlists.Any())
            {
                return NotFound();
            }
            var songlistDtos = songlists.Select(s => new SonglistModel
            {
                SonglistId = s.SonglistId,
                SonglistName = s.SonglistName,
                SongCount = s.SongCount
            }).ToList();

            // 返回 JSON 格式的 DTO 列表
            return Ok(songlistDtos);
        }

        // 获取某歌单中的所有歌曲
        [Authorize]
        [HttpGet("{songlistId}/songs")]
        public async Task<IActionResult> GetSongsInSonglist(string songlistId)
        {
            var songs = await _songlistService.GetSongsInSonglistAsync(songlistId);
            if (songs == null || !songs.Any())
            {
                return NotFound();
            }
            return Ok(songs);
        }

        //增加歌单
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddSonglist([FromBody] CreateSonglistModel model)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }

            // 创建一个 Songlist 实体并填充需要的属性
            var songlist = new Songlist
            {
                SonglistId = model.SonglistId,
                SonglistName = model.SonglistName,
                SonglistDate = model.SonglistDate,
                SonglistIspublic = model.IsPublic,
                UserId = userId
            };

            var newSonglistId = await _songlistService.AddSonglistAsync(songlist);
            return CreatedAtAction(nameof(GetSongsInSonglist), new { songlistId = newSonglistId }, newSonglistId);
        }

        //删除歌单
        [Authorize]
        [HttpDelete("{songlistId}/delete")]
        public async Task<IActionResult> DeleteSonglist(string songlistId)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }
            var result = await _songlistService.DeleteSonglistAsync(songlistId, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // 向歌单中添加歌曲
        [Authorize]
        [HttpPost("{songlistId}/songs/{songId}/add")]
        public async Task<IActionResult> AddSongToSonglist(string songlistId, string songId)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }
            var result = await _songlistService.AddSongToSonglistAsync(songlistId, songId, userId);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        // 从歌单中删除歌曲
        [Authorize]
        [HttpDelete("{songlistId}/songs/{songId}/delete")]
        public async Task<IActionResult> DeleteSongFromSonglist(string songlistId, string songId)
        {
            string userId="001";
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }
            var result = await _songlistService.DeleteSongFromSonglistAsync(songlistId, songId, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}