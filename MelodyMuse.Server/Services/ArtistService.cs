using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name)
        {
            return await _artistRepository.GetArtistsByNameAsync(name);
        }
        public async Task<int> GetArtistFansCountAsync(string artistId)
        {
            return await _artistRepository.GetArtistFansCountAsync(artistId);
        }
    }
}
