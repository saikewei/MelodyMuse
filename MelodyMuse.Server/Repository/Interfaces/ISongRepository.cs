using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface ISongRepository
    {
        Task<bool> CreateSongAsync(Song song);
        Task<Song> GetSongByNameAndAlbumAsync(string songName, string albumId);
        // 你可以添加更多的接口方法，例如获取所有歌曲
    }
}
