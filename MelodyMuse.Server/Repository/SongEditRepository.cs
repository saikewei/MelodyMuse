using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MelodyMuse.Server.Repository
{
    public class SongEditRepository : ISongEditRepository
    {
        private readonly ModelContext _context;

        public SongEditRepository()
        {
            _context = new ModelContext();
        }

        public async Task<Song> GetSongByIdAsync(string id)
        {
            return await _context.Songs
                    .Include(s => s.Composer)
                    .FirstOrDefaultAsync(s => s.SongId == id);
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<SongUpdateModel>> GetAllSongsAsync()
        {
            return await _context.Songs
                     .Include(s => s.Composer)
                     .Include(s => s.Artists)
                     .Select(s => new SongUpdateModel
                     {
                         SongId = s.SongId,
                         SongName = s.SongName,
                         SongGenre = s.SongGenre,
                         Duration = s.Duration.HasValue ? s.Duration.Value : 0,                         SongDate = s.SongDate,
                         SingerName = s.Artists.Select(a => a.ArtistName).ToList(),
                         ComposerName = s.Composer.ArtistName // 返回歌手信息
                     })
                     .ToListAsync();
        }
    }
}
