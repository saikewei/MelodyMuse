using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;


namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/songedit")]
    public class SongEditController : Controller
    {
        private readonly ISongEditService _songEditService;

        public SongEditController(ISongEditService songEditService)
        {
            _songEditService = songEditService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Song>>> GetAllSongs()
        {
            var songs = await _songEditService.GetAllSongsAsync();
            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSongById(string id)
        {
            var song = await _songEditService.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            return Ok(song);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(string id, [FromBody] SongUpdateModel songDto)
        {
            try
            {
                await _songEditService.UpdateSongAsync(id, songDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
