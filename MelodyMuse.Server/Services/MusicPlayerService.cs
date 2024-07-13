using MelodyMuse.Server.models;
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
        public async Task<SongMetaDataModel> GetSongBySongId(string SongId)
        {
            var song = await _musicplayerrepository.GetSongBySongId(SongId);
            var singers = await _musicplayerrepository.GetSingersBySongId(SongId);
            List<string?> singerNames = singers.Select(o => o.ArtistName).ToList();    

            var responseModel = new SongMetaDataModel();
            responseModel.song_id = song.SongId;
            responseModel.song_name = song.SongName;
            responseModel.singer_names = singerNames;
            responseModel.song_genre = song.SongGenre;
            responseModel.song_date = song.SongDate;
            responseModel.song_duration = song.Duration;
            responseModel.composer_name = song.Composer != null ? song.Composer.ArtistName : null;

            return responseModel;
        }
    }
}
