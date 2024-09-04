using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api")]
    public class StatisticController : Controller
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        [Authorize]
        [HttpGet("favorite-genres")]
        public async Task<IActionResult> GetUserFavoriteGenres()
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
                Console.WriteLine("Token error! " + ex);
                return Unauthorized();
            }

            var favoriteGenres = await _statisticService.GetUserFavoriteGenresAsync(userId);

            if (favoriteGenres == (null, null))
            {
                return NotFound("No data available for this user.");
            }

            return Ok(new
            {
                FavoriteSongGenre = favoriteGenres.favoriteSongGenre,
                FavoriteArtistGenre = favoriteGenres.favoriteArtistGenre
            });
        }

    }
}
