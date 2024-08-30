using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.models
{
    public class SongModel
    {
        public string SongId { get; set; } = null!;

        public string? SongName { get; set; }

        public string? SongGenre { get; set; }

        public decimal? Duration { get; set; }

        public string? Lyrics { get; set; }

        public DateTime? SongDate { get; set; }

        public string? ComposerId { get; set; }

        public byte? Status { get; set; }

        public string? AlbumId { get; set; }

        public List<Artist>? Artists { get; set; }
    }

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
