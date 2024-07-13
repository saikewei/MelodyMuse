using MelodyMuse.Server.Models;
/*
   MusicPlayer服务提供的接口(IAccountService)
 */

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IMusicPlayerService
    {
        Task<Song> GetSongBySongId(string songId);
    }
}
