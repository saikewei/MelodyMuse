using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface ISearchRepository
    {
        Task<List<Artist>> SearchArtists(string query);
        Task<List<SongModel>> SearchSongsByName(string query);
        Task<List<SongSearchModel>> SearchSongsByLyrics(string query);
        Task<List<Artist>> GetArtistsBySongId(string songId);
    }
}
