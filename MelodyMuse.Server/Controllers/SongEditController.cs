using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using Microsoft.AspNetCore.Cors;


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
        public async Task<ActionResult<SongUpdateModel>> GetSongById(string id)
        {
            var song = await _songEditService.GetSongByIdAsync(id);
            if (song == null)
                return null;
            var songDto = new SongUpdateModel
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SongGenre = song.SongGenre,
                SongDate = song.SongDate,
                Lyrics = song.Lyrics,
                ComposerName = song.Composer != null ? song.Composer.ArtistName : "Unknown",
                SingerName = song.Artists.Select(a => a.ArtistName).ToList(),
                Duration = song.Duration.HasValue ? song.Duration.Value : 0
            };

            return Ok(songDto);
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
