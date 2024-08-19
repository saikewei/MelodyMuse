using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<AlbumDto>> GetAllAlbumsByArtistIdAsync(string artistId)
        {
            return await _albumRepository.GetAllAlbumsByArtistIdAsync(artistId);
        }
    }
}
