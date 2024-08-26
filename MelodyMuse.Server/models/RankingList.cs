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
public class ArtistRankingData
    {
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public decimal TotalPlayCount { get; set; }
        public int FansCount { get; set; }
    }

    public class ArtistRankingDto
    {
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public decimal TotalPlayCount { get; set; }
        public int FansCount { get; set; }
        public decimal RankScore { get; set; }
    }

}
