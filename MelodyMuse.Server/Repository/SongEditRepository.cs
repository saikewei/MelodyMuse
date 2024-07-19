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
            return await _context.Songs.FindAsync(id);
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Song>> GetAllSongsAsync()
        {
            return await _context.Songs
                    .Select(s => new Song
                    {
                        SongId = s.SongId,
                        SongName = s.SongName,
                        SongGenre = s.SongGenre,
                        Lyrics = s.Lyrics,
                        SongDate = s.SongDate
                    })
                    .ToListAsync();
        }
    }
}
