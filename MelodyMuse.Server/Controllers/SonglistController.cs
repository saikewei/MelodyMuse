using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using Microsoft.AspNetCore.Cors;


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
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserSonglists(string userId)
        {
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