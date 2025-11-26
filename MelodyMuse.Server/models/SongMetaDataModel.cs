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

        // 作曲家ID
        public string? ComposerId { get; set; }

        // 作曲家名称
        public string? ComposerName { get; set; }

        // 歌手名称列表
        public List<string?>? SingerNames { get; set; }

        //所属专辑ID
        public string? AlbumId { get; set; }

        // 歌曲链接
        public string? SongUrl { get; set; }
        // 封面链接
        public string? CoverUrl { get; set; }
        // 歌词链接
        public string? LyricUrl { get; set; }

        // 1. 定义一个静态的“空实例”，供全局复用
        public static readonly SongMetaDataModel Null = new NullSongMetaDataModel();

        // 2. 添加一个属性来判断是否为空 (默认不是)
        public virtual bool IsNull => false;
    }

    public class NullSongMetaDataModel : SongMetaDataModel
    {
        public NullSongMetaDataModel()
        {
            // 初始化为“安全”的默认值，防止前端渲染出错
            SongId = string.Empty;
            SongName = "未知歌曲";
            SingerNames = new List<string>();
            SongGenre = "Unknown";
            ComposerName = "未知";
            AlbumId = string.Empty;
        }

        // 重写属性，标记为 true
        public override bool IsNull => true;
    }
}