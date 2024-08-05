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

        // 获取海报
        [HttpGet("{id}/poster")]
        public async Task<IActionResult> GetPoster(string id)
        {
            var posterStream = await _songEditService.GetPosterAsync(id);
            if (posterStream == null)
                return NotFound();

            return File(posterStream, "image/png");
        }

        // 上传海报
        [HttpPost("{id}/poster")]
        public async Task<IActionResult> UploadPoster(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("Invalid file type.");

            try
            {
                await _songEditService.SavePosterAsync(id, file);
                return Ok(new { message = "Poster uploaded successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
