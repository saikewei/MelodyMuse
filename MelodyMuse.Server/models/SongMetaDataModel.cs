namespace MelodyMuse.Server.Models
{
    // 歌曲元数据模型类
    public class SongMetaDataModel
    {
        // 歌曲名称
        public string? SongName { get; set; } = string.Empty;

        // 歌曲ID
        public string SongId { get; set; } = string.Empty;

        // 歌曲时长
        public decimal? SongDuration { get; set; }

        // 发行日期
        public DateTime? SongDate { get; set; }

        // 歌曲类型
        public string? SongGenre { get; set; }

        // 作曲家名称
        public string? ComposerName { get; set; }

        // 歌手名称列表
        public List<string?>? SingerNames { get; set; }

        // 歌曲链接
        public string? SongUrl { get; set; }
    }
}
