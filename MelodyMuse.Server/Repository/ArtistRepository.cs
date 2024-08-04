using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;

namespace MelodyMuse.Server.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ModelContext _context;

        public ArtistRepository()
        {
            _context = new ModelContext();
        }

        public async Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name)
        {
            return await _context.Artists
                .Where(a => a.ArtistName.Contains(name))
                .ToListAsync();
        }

        public async Task<Artist> GetArtistByIdAsync(string artistId)
        {
            return await _context.Artists
                .FirstOrDefaultAsync(a => a.ArtistId == artistId);
        }

        public async Task<bool> artistSingSongAsync(string songId, string artistId)
        {
            var sql = "INSERT INTO ARTIST_SING_SONG (SONG_ID, ARTIST_ID) VALUES (:songId, :artistId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("songId", songId), new OracleParameter("artistId", artistId));
            return result > 0;
        }
    }
}
