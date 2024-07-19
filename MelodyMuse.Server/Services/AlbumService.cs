using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsByArtistIdAsync(string artistId)
        {
            return await _albumRepository.GetAllAlbumsByArtistIdAsync(artistId);
        }
    }
}
