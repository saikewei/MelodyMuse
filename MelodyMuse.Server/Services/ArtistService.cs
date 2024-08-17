using MelodyMuse.Server.models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace MelodyMuse.Server.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<Artist> GetArtistByIdAsync(string artistId)
        {
            return await _artistRepository.GetArtistByIdAsync(artistId);
        }

        public async Task<List<Song>> GetSongsByArtistIdAsync(string artistId)
        {
            return await _artistRepository.GetSongsByArtistIdAsync(artistId);
        }
    }
}