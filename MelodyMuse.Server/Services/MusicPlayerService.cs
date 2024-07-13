using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

/*
  MusicPlayer服务层的函数实现(调用下一层repository提供的接口)
 */

namespace MelodyMuse.Server.Services
{
    // MusicPlayer服务类，实现IMusicPlayerService接口
    public class MusicPlayerService : IMusicPlayerService
    {
        // 依赖的音乐播放器仓库接口
        private readonly IMusicPlayerRepository _musicplayerrepository;

        // 构造函数，注入IMusicPlayerRepository实例
        public MusicPlayerService(IMusicPlayerRepository musicplayerrepository)
        {
            _musicplayerrepository = musicplayerrepository;
        }

        // 根据歌曲ID获取歌曲元数据
        public async Task<SongMetaDataModel> GetSongBySongId(string songId)
        {
            // 从仓库层获取歌曲信息
            var song = await _musicplayerrepository.GetSongBySongId(songId);
            // 从仓库层获取歌手信息
            var singers = await _musicplayerrepository.GetSingersBySongId(songId);
            // 提取歌手名称列表
            List<string?> singerNames = singers.Select(o => o.ArtistName).ToList();

            // 创建并填充SongMetaDataModel对象
            var responseModel = new SongMetaDataModel
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SingerNames = singerNames,
                SongGenre = song.SongGenre,
                SongDate = song.SongDate,
                SongDuration = song.Duration,
                ComposerName = song.Composer != null ? song.Composer.ArtistName : null
            };

            // 返回填充好的SongMetaDataModel对象
            return responseModel;
        }
    }
}
