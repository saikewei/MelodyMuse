using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MelodyMuse.Server.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ModelContext _context;

        public ArtistRepository(ModelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name)
        {
            return await _context.Artists
                .Where(a => a.ArtistName.Contains(name))
                .ToListAsync();
        }
    }
}
