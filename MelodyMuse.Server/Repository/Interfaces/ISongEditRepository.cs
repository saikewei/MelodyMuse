using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface ISongEditRepository
    {
        Task<Song> GetSongByIdAsync(string id);
        Task UpdateSongAsync(Song song);
        Task<IList<SongUpdateModel>> GetAllSongsAsync();
        string GetPosterPath(string id);
    }
}
