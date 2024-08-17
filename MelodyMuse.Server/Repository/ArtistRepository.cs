using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace MelodyMuse.Server.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ModelContext _context;

        public ArtistRepository()
        {
            _context = new ModelContext();
        }

        public async Task<Artist> GetArtistByIdAsync(string artistId)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId);
        }

        public async Task<List<Song>> GetSongsByArtistIdAsync(string artistId)
        {
            // Assuming there is a many-to-many relationship between Songs and Artists
            return await _context.Songs
                .Where(song => song.Artists.Any(artist => artist.ArtistId == artistId))
                .ToListAsync();
        }
         public async Task<bool> FollowArtistAsync(string userId, string artistId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Artists)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                
                var artist = await _context.Artists
                    .FirstOrDefaultAsync(a => a.ArtistId == artistId);

                if (user == null || artist == null)
                {
                    return false;
                }

                if (!user.Artists.Contains(artist))
                {
                    user.Artists.Add(artist);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
