
//歌曲审核
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using System.Threading.Tasks;

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
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingApprovalSongs()
        {
            var songs = await _songService.GetPendingApprovalSongsAsync();
            return Ok(songs);
        }

        // 审核通过歌曲
        [HttpPost("{songId}/approve")]
        public async Task<IActionResult> ApproveSong(string songId)
        {
            await _songService.ApproveSongAsync(songId);
            return Ok(new { msg = "歌曲审核通过" });
        }

        // 审核不通过歌曲
        [HttpPost("{songId}/reject")]
        public async Task<IActionResult> RejectSong(string songId)
        {
            await _songService.RejectSongAsync(songId);
            return Ok(new { msg = "歌曲审核不通过" });
        }
    }
}
