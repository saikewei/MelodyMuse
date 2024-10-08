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
            var _artist = await _artistRepository.GetArtistByIdAsync(artistId);
            _artist.ArtistFansNum = await _artistRepository.GetArtistFansCountAsync(artistId);
            return await _artistRepository.GetArtistByIdAsync(artistId);
        }

        public async Task<List<Song>> GetSongsByArtistIdAsync(string artistId)
        {
            return await _artistRepository.GetSongsByArtistIdAsync(artistId);
        }
        public async Task<bool> FollowArtistAsync(string userId, string artistId)
        {
            return await _artistRepository.FollowArtistAsync(userId, artistId);
        }

        public async Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name)
        {
            return await _artistRepository.GetArtistsByNameAsync(name);
        }
        public async Task<bool> UnfollowArtistAsync(string userId, string artistId)
        {
            return await _artistRepository.UnfollowArtistAsync(userId, artistId);
        }
        public async Task<bool> IncrementArtistFansNumAsync(string artistId)
        {
            return await _artistRepository.IncrementArtistFansNumAsync(artistId);
        }
        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            return await _artistRepository.GetAllArtistsAsync();
        }
        public async Task<List<Artist>> GetArtistsByUserIdAsync(string userId)
        {
            return await _artistRepository.GetArtistsByUserIdAsync(userId);
        }
        public async Task<int> GetArtistFansCountAsync(string artistId)
        {
            return await _artistRepository.GetArtistFansCountAsync(artistId);
        }

        public async Task<bool> IsFollowingArtistAsync(string userId, string artistId)
        {
            return await _artistRepository.IsFollowingArtistAsync(userId, artistId);
        }

    }
}
