using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepository;
        public StatisticService(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }
        public async Task<(string favoriteSongGenre, string favoriteArtistGenre)> GetUserFavoriteGenresAsync(string userId)
        {
            return await _statisticRepository.GetUserFavoriteGenresAsync(userId);
        }
    }
}
