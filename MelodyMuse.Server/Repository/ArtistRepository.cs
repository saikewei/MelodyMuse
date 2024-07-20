using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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
    }
}
