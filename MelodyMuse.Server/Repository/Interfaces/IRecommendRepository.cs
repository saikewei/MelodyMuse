/*
  歌曲推荐功能提供的服务接口
*/

using MelodyMuse.Server.Models;


namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IRecommendRepository
    {
        Task<List<SongPlayCount>> GetSongPlayCountById(string userId);
        Task<List<Song>> GetAllSongs();
    }
}
