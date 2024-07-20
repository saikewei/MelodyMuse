namespace MelodyMuse.Server.models
{
    // 用于歌曲更新界面获取歌曲信息
    public class SongUpdateModel
    {
        public string? SongId { get; set; }
        public string? SongName { get; set; }
        public string? SongGenre { get; set; }
        public decimal? Duration { get; set; }
        public DateTime? SongDate { get; set; }
        public List<string>? SingerName {  get; set; }
        public string? ComposerName { get; set; }
        public string? Lyrics { get; set; }
    }
}
