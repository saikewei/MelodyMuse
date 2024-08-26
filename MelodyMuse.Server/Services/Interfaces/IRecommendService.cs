using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IRecommendService
    {
        Task<List<SongPlayCount>> GetSongPlayCountById(string userId);
        Task<List<Song>> GetAllSongs();
    }
}
