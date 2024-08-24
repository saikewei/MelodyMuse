using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;
namespace MelodyMuse.Server.Controllers
{
    [Route("api/artistnum")]
    [ApiController]
    public class ArtistNumController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistNumController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("{artistId}/fans-count")]
        public async Task<IActionResult> GetArtistFansCount(string artistId)
        {
            var fansCount = await _artistService.GetArtistFansCountAsync(artistId);
            return Ok(new { ArtistId = artistId, FansCount = fansCount });
        }
    }
}