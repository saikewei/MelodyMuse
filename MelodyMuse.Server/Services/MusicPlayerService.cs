using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

/*
  MusicPlayer服务层的函数实现(调用下一层repository提供的接口)
 */


namespace MelodyMuse.Server.Services
{
    public class MusicPlayerService : IMusicPlayerService
    {
        private readonly IMusicPlayerRepository _musicplayerrepository;

        public MusicPlayerService(IMusicPlayerRepository musicplayerrepository)
        {
            _musicplayerrepository = musicplayerrepository; 
        }
        public Song GetSongBySongId(string SongId)
        {
            return _musicplayerrepository.GetSongBySongId(SongId);
        }
    }
}
