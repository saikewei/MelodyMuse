using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IArtistService
    {
        Task<Artist> GetArtistByIdAsync(string artistId);
        Task<List<Song>> GetSongsByArtistIdAsync(string artistId);
        Task<bool> FollowArtistAsync(string userId, string artistId);

        Task<bool> UnfollowArtistAsync(string userId, string artistId);
        Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name);
    }
}
