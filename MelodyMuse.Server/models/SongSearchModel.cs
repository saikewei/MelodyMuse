namespace MelodyMuse.Server.models
{
    //搜索歌词用
    public class SongSearchModel
    {
        public string? SongId { get; set; }
        public string? SongName { get; set; }
        public List<MatchedLyricsLine> MatchedLyricsLines { get; set; } = new List<MatchedLyricsLine>();
    }

    public class MatchedLyricsLine
    {
        public int LineNumber { get; set; }
        public string? LyricsLine { get; set; }
    }
}
