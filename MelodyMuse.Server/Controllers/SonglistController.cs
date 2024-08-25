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
    public class SonglistController : ControllerBase
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
                Console.WriteLine("token错误！"+ex);
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
    }

}