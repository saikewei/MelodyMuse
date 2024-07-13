using MelodyMuse.Server.Models;
/*
   MusicPlayerRepository提供的服务接口
 */

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IMusicPlayerRepository
    {
        Task<Song> GetSongBySongId(string songId);
    }
}
