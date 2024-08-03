using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MelodyMuse.Server.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly ModelContext _context;

        public SongRepository()
        {
            _context = new ModelContext();
        }

        public async Task<bool> CreateSongAsync(Song song)
        {
            _context.Songs.Add(song);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Song> GetSongByNameAndAlbumAsync(string songName, string albumId)
        {
            return await _context.Songs
                .Include(s => s.Albums)
                .FirstOrDefaultAsync(s => s.SongName == songName && s.Albums.Any(a => a.AlbumId == albumId));
        }

        
    }
}
