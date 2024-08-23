namespace MelodyMuse.Server.models
{
public class SongRankingDto
{
 public string SongId { get; set; } = null!;
        public string? SongName { get; set; }
        public decimal? Duration { get; set; }
        public string? ArtistName { get; set; }
        public decimal PlayCount { get; set; }
}

}
