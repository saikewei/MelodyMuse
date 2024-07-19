using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface ISongEditService
    {
        Task<Song> GetSongByIdAsync(string id);
        Task UpdateSongAsync(string id, SongUpdateModel songDto);
        Task<IList<Song>> GetAllSongsAsync();
    }
}