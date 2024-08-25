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
using Oracle.ManagedDataAccess.Client;
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
              var sql = "INSERT INTO USER_FOLLOW_ARTIST (USER_ID, ARTIST_ID) VALUES (:userId, :artistId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("userId", userId), new OracleParameter("artistId", artistId));
            return result > 0;
            /*try
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
            }*/
            
        }
    

         public async Task<bool> UnfollowArtistAsync(string userId, string artistId)
    {
        var sql = "DELETE FROM USER_FOLLOW_ARTIST WHERE USER_ID = :userId AND ARTIST_ID = :artistId";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("userId", userId), new OracleParameter("artistId", artistId));
            return result > 0;
    }

        public async Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name)
        {
            return await _context.Artists
                .Where(a => a.ArtistName.Contains(name))
                .ToListAsync();
        }



        public async Task<bool> artistSingSongAsync(string songId, string artistId)
        {
            var sql = "INSERT INTO ARTIST_SING_SONG (SONG_ID, ARTIST_ID) VALUES (:songId, :artistId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("songId", songId), new OracleParameter("artistId", artistId));
            return result > 0;
        }

        public async Task<bool> IncrementArtistFansNumAsync(string artistId)
    {
        try
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId);
            if (artist != null)
            {
                artist.ArtistFansNum = (artist.ArtistFansNum ?? 0) + 1;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

         public async Task<List<Artist>> GetAllArtistsAsync()
    {
        return await _context.Artists.ToListAsync();
    }

     public async Task<List<Artist>> GetArtistsByUserIdAsync(string userId)
    {
        return await _context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Artists)
            .ToListAsync();
    }

    }
}
