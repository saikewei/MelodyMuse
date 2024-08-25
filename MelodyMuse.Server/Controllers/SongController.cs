
//歌曲审核
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/songs")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        // 获取待审核的歌曲
         [Authorize]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingApprovalSongs()
        {
            var songs = await _songService.GetPendingApprovalSongsAsync();
            return Ok(songs);
        }

        // 审核通过歌曲
         [Authorize]
        [HttpPost("{songId}/approve")]
        public async Task<IActionResult> ApproveSong(string songId)
        {
            await _songService.ApproveSongAsync(songId);
            return Ok(new { msg = "歌曲审核通过" });
        }

        // 审核不通过歌曲
         [Authorize]
        [HttpPost("{songId}/reject")]
        public async Task<IActionResult> RejectSong(string songId)
        {
            await _songService.RejectSongAsync(songId);
            return Ok(new { msg = "歌曲审核不通过" });
        }

        // 查询作词家所作的所有歌曲
         [Authorize]
        [HttpGet("composer/{composerId}")]
        public async Task<IActionResult> GetSongsByComposerId(string composerId)
        {
            var songs = await _songService.GetSongsByComposerIdAsync(composerId);

            if (songs == null || songs.Count== 0)
            {
                return NotFound(new { message = "没有找到相关歌曲。" });
            }

            return Ok(songs);
        }
         [Authorize]
        [HttpGet("{songId}/album")]
        public async Task<IActionResult> GetAlbumBySongId(string songId)
        {
            var result = await _songService.GetAlbumBySongIdAsync(songId);

            if (result == "Song not found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
