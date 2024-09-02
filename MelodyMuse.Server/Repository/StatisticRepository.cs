using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MelodyMuse.Server.Repository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly ModelContext _context;

        public StatisticRepository()
        {
            _context = new ModelContext();
        }
        public async Task<(string favoriteSongGenre, string favoriteArtistGenre)> GetUserFavoriteGenresAsync(string userId)
        {
            var songPlayData = await (from play in _context.SongPlayCounts
                                      join song in _context.Songs on play.SongId equals song.SongId
                                      where play.UserId == userId
                                      select new
                                      {
                                          play.Count,
                                          song.SongGenre,
                                          song.Artists
                                      }).ToListAsync();

            // 统计用户最喜欢的歌曲流派
            var favoriteSongGenre = songPlayData
                .GroupBy(s => s.SongGenre)
                .OrderByDescending(g => g.Sum(s => s.Count))
                .Select(g => g.Key)
                .FirstOrDefault();

            // 统计用户最喜欢的艺术家流派
            var artistGenres = songPlayData
                .SelectMany(s => s.Artists.Select(a => new { a.ArtistIntro, s.Count }))
                .GroupBy(a => a.ArtistIntro)
                .OrderByDescending(g => g.Sum(a => a.Count))
                .Select(g => g.Key)
                .FirstOrDefault();

            return (favoriteSongGenre, artistGenres);
        }
    }
}
