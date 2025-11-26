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
            // 1. 获取歌曲信息
            var song = await _musicplayerrepository.GetSongBySongId(songId);

            // 【应用空对象模式】
            // 如果查不到歌曲，不再抛异常炸毁程序，而是返回一个安全的 Null 对象。
            // 调用者检查 IsNull 即可知道结果。
            if (song == null)
            {
                return SongMetaDataModel.Null;
            }

            // 2. 获取歌手信息
            var singers = await _musicplayerrepository.GetSingersBySongId(songId);

            // 【应用空对象思想 - 局部处理】
            // 如果歌手列表为空，不报错，而是给一个包含“未知歌手”的默认列表。
            // 这样保证 SingerNames 永远不为 null，前端直接遍历不会崩。
            var singerNames = (singers == null || !singers.Any())
                ? new List<string> { "未知歌手" }
                : singers.Select(o => o.ArtistName ?? "匿名").ToList();

            // 3. 获取专辑信息 (处理可能的 null)
            var albumId = await _musicplayerrepository.GetAlbumIdBySongId(songId);

            // 4. 构建返回对象
            var responseModel = new SongMetaDataModel
            {
                SongId = song.SongId,
                SongName = song.SongName ?? "无标题", // 防止数据库脏数据
                SingerNames = singerNames,
                SongGenre = song.SongGenre,
                SongDate = song.SongDate,
                SongDuration = song.Duration,
                ComposerId = song.ComposerId,
                ComposerName = song.Composer?.ArtistName ?? "未知作曲家", // 防止空引用
                AlbumId = albumId ?? string.Empty // 确保不为 null
            };

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
