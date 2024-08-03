using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MelodyMuse.Server.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ModelContext _context;

        public AlbumRepository()
        {
            _context = new ModelContext();
        }

       
        public async Task<bool> CreateAlbumAsync(Album album)
        {
            _context.Albums.Add(album);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Album> GetAlbumByIdAsync(string albumId)
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.AlbumId == albumId);
        }


        public async Task<IEnumerable<Album>> GetAllAlbumsByArtistIdAsync(string artistId)
        {
            return await _context.Albums
                .Where(a => a.ArtistId == artistId)
                //.Include(a=>a.Artist)
                .Include(a => a.Songs)
                .ToListAsync();
        }
    }

}
