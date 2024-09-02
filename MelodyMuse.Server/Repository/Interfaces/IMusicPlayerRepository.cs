using MelodyMuse.Server.Models;
/*
   MusicPlayerRepository提供的服务接口
 */

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IMusicPlayerRepository
    {
        Task<Song> GetSongBySongId(string songId);
        Task<List<Artist>> GetSingersBySongId(string songId);

        Task<string> GetAlbumIdBySongId(string songId);

        Task<bool> CountPlays(string songId, string userId);

        Task<bool> DeleteCountPlaysRecord(string songId);
    }
}
