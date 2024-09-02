using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using TencentCloud.Ame.V20190916.Models;

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
            if (song == null)
            {
                throw new ArgumentNullException($"Song with ID {songId} not found.");
            }

            // 从仓库层获取歌手信息
            var singers = await _musicplayerrepository.GetSingersBySongId(songId);
            if (singers == null || !singers.Any())
            {
                throw new ArgumentNullException($"Singers for song with ID {songId} not found.");
            }

            // 提取歌手名称列表
            var singerNames = singers.Select(o => o.ArtistName).Where(name => name != null).ToList();

            // 获取歌曲所属专辑ID
            var albumId = await _musicplayerrepository.GetAlbumIdBySongId(songId);
            /*if (albumId == null)
            {
                throw new ArgumentNullException($"Album for song with ID {songId} not found.");
            }*/

            // 创建并填充SongMetaDataModel对象
            var responseModel = new SongMetaDataModel
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SingerNames = singerNames,
                SongGenre = song.SongGenre,
                SongDate = song.SongDate,
                SongDuration = song.Duration,
                ComposerId = song.ComposerId,
                ComposerName = song.Composer?.ArtistName,
                AlbumId = albumId
            };

            // 返回填充好的SongMetaDataModel对象
            return responseModel;
        }

        public async Task<bool> IncreaseSongPlaysBySongIdandUserId(string songId, string userId)
        {
            if (string.IsNullOrEmpty(songId))
            {
                throw new ArgumentNullException(nameof(songId), "The provided song ID cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "The provided user ID cannot be null or empty.");
            }

            return await _musicplayerrepository.CountPlays(songId, userId);
        }

    }
}
