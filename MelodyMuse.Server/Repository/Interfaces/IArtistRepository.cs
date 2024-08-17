using System.Threading.Tasks;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Collections.Generic;
namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IArtistRepository
    {
        Task<Artist> GetArtistByIdAsync(string artistId);
         Task<List<Song>> GetSongsByArtistIdAsync(string artistId);
         Task<bool> FollowArtistAsync(string userId, string artistId);
    }
}